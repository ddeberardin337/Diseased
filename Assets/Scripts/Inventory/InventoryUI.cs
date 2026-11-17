
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject inventoryPanel;

    [Header("Inventory Slots")]
    [SerializeField] private TMP_Text[] itemNameTexts;
    [SerializeField] private TMP_Text[] quantityTexts;
    [SerializeField] private Button[] slotButtons;

    [Header("Item Details")]
    [SerializeField] private TMP_Text selectedItemName;
    [SerializeField] private TMP_Text selectedItemDescription;
    [SerializeField] private TMP_Text selectedItemEffect;
    [SerializeField] private GameObject useButton;

    [Header("Navigation")]
    [SerializeField] private InventoryPanel inventoryPanelController;
    [SerializeField] private Button backButton;

    private Inventory inventory;
    private PlayerHealth playerHealth;
    private PlayerDisease playerDisease;

    private ItemData selectedItem;
    private Button useItemButton;

    private float previousTimeScale = 1f;

    private void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        playerDisease = FindFirstObjectByType<PlayerDisease>();

        if (inventoryPanel != null)
            inventoryPanel.SetActive(false);

        SetupSlotButtons();

        backButton.onClick.AddListener(GoBackToInventory);

        if (useButton != null)
        {
            useItemButton = useButton.GetComponent<Button>();

            if (useItemButton != null)
                useItemButton.onClick.AddListener(UseSelectedItem);
        }

        RefreshInventory();
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.iKey.wasPressedThisFrame)
        {
            ToggleInventory();
        }
    }

    private void SetupSlotButtons()
    {
        for (int i = 0; i < slotButtons.Length; i++)
        {
            int slotIndex = i;

            if (slotButtons[i] == null)
                continue;

            slotButtons[i].onClick.RemoveAllListeners();

            slotButtons[i].onClick.AddListener(() =>
            {
                SelectItem(slotIndex);
            });
        }
    }

    private void SelectItem(int index)
    {
        if (inventory == null)
            return;

        if (index < 0 || index >= inventory.slots.Count)
            return;

        InventorySlot slot = inventory.slots[index];

        if (slot.itemData == null)
            return;

        selectedItem = slot.itemData;

        selectedItemName.text = selectedItem.itemName;
        selectedItemDescription.text = selectedItem.description;
        selectedItemEffect.text = selectedItem.effectDescription;

        useButton.SetActive(selectedItem.isUsable);

        inventoryPanelController.ShowItemDetails();
    }

    private void UseSelectedItem()
    {
        if (selectedItem == null)
            return;

        if (!selectedItem.isUsable)
            return;

        // Make sure the player still owns the item.
        if (!inventory.HasItem(selectedItem))
        {
            Debug.LogWarning("Item is no longer in inventory!");
            return;
        }

        // Consume the item first if required.
        if (selectedItem.consumeOnUse)
        {
            if (!inventory.RemoveItem(selectedItem, 1))
            {
                Debug.LogWarning("Could not consume item!");
                return;
            }
        }

        // Apply the item's effect.
        switch (selectedItem.effectType)
        {
            case ItemEffectType.None:
                Debug.Log("This item has no effect.");
                break;

            case ItemEffectType.Heal:
                if (playerHealth != null)
                    playerHealth.Heal(selectedItem.effectAmount);
                break;

            case ItemEffectType.Damage:
                if (playerHealth != null)
                    playerHealth.TakeDamage(selectedItem.effectAmount);
                break;

            case ItemEffectType.ReduceDiseaseDamage:
                if (playerDisease != null)
                {
                    playerDisease.ReduceDiseaseDamage(
                        selectedItem.effectAmount,
                        selectedItem.effectDuration
                    );
                }
                break;

            case ItemEffectType.PauseDiseaseDamage:
                if (playerDisease != null)
                {
                    playerDisease.SuppressDisease(
                        selectedItem.effectDuration
                    );
                }
                break;
        }

        Debug.Log("Used item: " + selectedItem.itemName);

        selectedItem = null;

        RefreshInventory();

        inventoryPanelController.ShowInventory();
    }

    private void ToggleInventory()
    {
        if (inventoryPanel == null)
            return;

        bool isOpening = !inventoryPanel.activeSelf;

        inventoryPanel.SetActive(isOpening);

        if (isOpening)
        {
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;

            RefreshInventory();
        }
        else
        {
            Time.timeScale = previousTimeScale;
        }
    }

    public void RefreshInventory()
    {
        if (inventory == null)
            inventory = FindFirstObjectByType<Inventory>();

        if (inventory == null)
        {
            Debug.LogError("Player inventory not found!");
            return;
        }

        // Clear all UI slots first.
        for (int i = 0; i < itemNameTexts.Length; i++)
        {
            if (itemNameTexts[i] != null)
                itemNameTexts[i].text = "";

            if (i < quantityTexts.Length &&
                quantityTexts[i] != null)
            {
                quantityTexts[i].text = "";
            }
        }

        // Populate occupied slots.
        for (int i = 0;
             i < inventory.slots.Count && i < itemNameTexts.Length;
             i++)
        {
            InventorySlot slot = inventory.slots[i];

            if (slot.itemData == null)
                continue;

            if (itemNameTexts[i] != null)
                itemNameTexts[i].text = slot.itemData.itemName;

            if (i < quantityTexts.Length &&
                quantityTexts[i] != null)
            {
                quantityTexts[i].text = "x" + slot.quantity;
            }
        }
    }

    private void GoBackToInventory()
    {
        selectedItem = null;
        inventoryPanelController.ShowInventory();
    }
}
