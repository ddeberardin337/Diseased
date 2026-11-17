
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemData itemData;
    public int quantity;

    public InventorySlot(ItemData item, int amount)
    {
        itemData = item;
        quantity = amount;
    }
}

public class Inventory : MonoBehaviour
{
    public List<InventorySlot> slots = new List<InventorySlot>();

    public void AddItem(ItemData item, int amount = 1)
    {
        if (item == null || amount <= 0)
            return;

        int maxStack = Mathf.Max(1, item.maxStackSize);

        // First, fill any existing stacks of this item.
        foreach (InventorySlot slot in slots)
        {
            if (slot.itemData != item || slot.quantity >= maxStack)
                continue;

            int spaceAvailable = maxStack - slot.quantity;
            int amountToAdd = Mathf.Min(amount, spaceAvailable);

            slot.quantity += amountToAdd;
            amount -= amountToAdd;

            if (amount <= 0)
                return;
        }

        // If necessary, create additional stacks.
        while (amount > 0)
        {
            int amountToAdd = Mathf.Min(amount, maxStack);

            slots.Add(new InventorySlot(item, amountToAdd));

            amount -= amountToAdd;
        }
    }

    public bool RemoveItem(ItemData item, int amount = 1)
    {
        Debug.Log("RemoveItem called for: " + item.itemName);
        if (item == null || amount <= 0)
            return false;

        foreach (InventorySlot slot in slots)
        {
            if (slot.itemData != item)
                continue;

            if (slot.quantity < amount)
                return false;

            slot.quantity -= amount;

            if (slot.quantity <= 0)
                slots.Remove(slot);

            return true;
        }

        return false;
    }

    public bool HasItem(ItemData item, int amount = 1)
    {
        int totalQuantity = 0;

        foreach (InventorySlot slot in slots)
        {
            if (slot.itemData == item)
                totalQuantity += slot.quantity;
        }

        return totalQuantity >= amount;
    }
}
