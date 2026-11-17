
using UnityEngine;

public class ItemPickup : MonoBehaviour, IUsable
{
    [SerializeField] private ItemData itemData;

    [Header("Persistence")]
    [SerializeField] private string pickupID;

    [SerializeField]
    private string interactionPrompt = "Press E to collect";

    public string InteractionPrompt => interactionPrompt;

    private void Start()
    {
        if (string.IsNullOrWhiteSpace(pickupID))
        {
            Debug.LogWarning("Pickup ID is missing!", this);
            return;
        }

        if (WorldStateManager.Instance == null)
        {
            Debug.LogError("WorldStateManager not found!", this);
            return;
        }

        // Check whether this pickup was collected previously.
        if (WorldStateManager.Instance.HasCollectedPickup(pickupID))
        {
            Destroy(gameObject);
        }
    }

    public void Interact()
    {
        Inventory inventory = FindFirstObjectByType<Inventory>();

        if (inventory == null || itemData == null)
        {
            Debug.LogWarning("Inventory or item data missing!");
            return;
        }

        if (WorldStateManager.Instance == null)
        {
            Debug.LogError("WorldStateManager not found!");
            return;
        }

        if (!WorldStateManager.Instance.RegisterCollectedPickup(pickupID))
        {
            Debug.LogWarning("This pickup was already collected!");
            return;
        }

        inventory.AddItem(itemData);

        Debug.Log("Collected and recorded: " + pickupID);

        Destroy(gameObject);
    }

    
}
