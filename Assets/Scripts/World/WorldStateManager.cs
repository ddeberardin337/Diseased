
using System.Collections.Generic;
using UnityEngine;

public class WorldStateManager : MonoBehaviour
{
    public static WorldStateManager Instance { get; private set; }

    private HashSet<string> collectedPickupIDs = new HashSet<string>();

    private void Awake()
    {
        // Prevent duplicate managers.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // Keep this object alive when changing scenes.
        DontDestroyOnLoad(gameObject);
    }

    public bool HasCollectedPickup(string pickupID)
    {
        return collectedPickupIDs.Contains(pickupID);
    }

    public bool RegisterCollectedPickup(string pickupID)
    {
        if (string.IsNullOrWhiteSpace(pickupID))
        {
            Debug.LogWarning("Pickup ID is missing!");
            return false;
        }

        return collectedPickupIDs.Add(pickupID);
    }
}
