
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemRow : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text effectDescriptionText;
    [SerializeField] private Button useButton;

    private ItemData itemData;
    private Inventory inventory;
    private PlayerHealth playerHealth;

    public void Setup(
        ItemData item,
        int quantity,
        Inventory playerInventory)
    {
        itemData = item;
        inventory = playerInventory;

        playerHealth = FindFirstObjectByType<PlayerHealth>();

        itemNameText.text = itemData.itemName;
        quantityText.text = "x" + quantity;

        descriptionText.text = itemData.description;
        effectDescriptionText.text = itemData.effectDescription;

        useButton.gameObject.SetActive(itemData.isUsable);

        useButton.onClick.RemoveAllListeners();
        useButton.onClick.AddListener(UseItem);
    }

    private void UseItem()
    {
        if (itemData == null || inventory == null)
            return;

        if (playerHealth == null)
            playerHealth = FindFirstObjectByType<PlayerHealth>();

        switch (itemData.effectType)
        {
            case ItemEffectType.Heal:
                if (playerHealth == null)
                {
                    Debug.LogWarning("PlayerHealth not found!");
                    return;
                }

                playerHealth.Heal(itemData.effectAmount);
                break;

            case ItemEffectType.Damage:
                if (playerHealth == null)
                {
                    Debug.LogWarning("PlayerHealth not found!");
                    return;
                }

                playerHealth.TakeDamage(itemData.effectAmount);
                break;

            case ItemEffectType.None:
                Debug.Log("This item has no direct effect.");
                break;
        }

        if (itemData.consumeOnUse)
            inventory.RemoveItem(itemData, 1);

        FindFirstObjectByType<InventoryUI>()?.RefreshInventory();
    }
}
