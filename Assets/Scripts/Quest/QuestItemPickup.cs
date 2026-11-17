
using UnityEngine;

public class QuestItemPickup : MonoBehaviour, IUsable
{
    [Header("Quest Item")]
    [SerializeField] private ItemData itemData;

    [Header("Persistence")]
    [SerializeField] private string pickupID;

    [SerializeField]
    private string interactionPrompt = "Press E to collect";

    public string InteractionPrompt => interactionPrompt;

    private void Start()
    {
        RefreshAvailability();
    }

    public void RefreshAvailability()
    {
        if (string.IsNullOrWhiteSpace(pickupID) || itemData == null)
        {
            Debug.LogError("Quest pickup references are missing!", this);
            gameObject.SetActive(false);
            return;
        }

        if (WorldStateManager.Instance == null ||
            QuestManager.Instance == null)
        {
            Debug.LogError("Quest or World State Manager missing!", this);
            gameObject.SetActive(false);
            return;
        }

        // Permanently remove this pickup for the current playthrough
        // if it has already been collected.
        if (WorldStateManager.Instance.HasCollectedPickup(pickupID))
        {
            Destroy(gameObject);
            return;
        }

        // Only show the object when the quest has unlocked it.
        gameObject.SetActive(
            QuestManager.Instance.IsItemUnlocked(itemData)
        );
    }

    public void Interact()
    {
        Inventory inventory = FindFirstObjectByType<Inventory>();

        if (inventory == null || itemData == null)
        {
            Debug.LogWarning("Inventory or quest item missing!");
            return;
        }

        if (QuestManager.Instance == null ||
            WorldStateManager.Instance == null)
        {
            Debug.LogError("Quest systems could not be found!");
            return;
        }

        if (!QuestManager.Instance.IsItemUnlocked(itemData))
        {
            Debug.Log("This quest item is not unlocked yet!");
            return;
        }

        if (!WorldStateManager.Instance.RegisterCollectedPickup(pickupID))
        {
            Debug.LogWarning("Quest item was already collected!");
            return;
        }

        inventory.AddItem(itemData);

        Debug.Log("Collected quest item: " + itemData.itemName);

        Destroy(gameObject);
    }
}
