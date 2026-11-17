
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    private Inventory inventory;
    private PlayerInteraction playerInteraction;

    private void Start()
    {
        inventory = FindFirstObjectByType<Inventory>();
        playerInteraction = FindFirstObjectByType<PlayerInteraction>();
    }

    public bool Craft(CraftingRecipe recipe)
    {
        if (recipe == null)
        {
            Debug.LogWarning("Crafting recipe is missing!");
            return false;
        }

        Debug.Log("CraftingSystem received recipe: " + recipe.name);

        if (inventory == null)
            inventory = FindFirstObjectByType<Inventory>();

        if (inventory == null)
        {
            Debug.LogWarning("Player inventory not found!");
            return false;
        }

        // Check that both ingredients exist.
        if (!inventory.HasItem(recipe.ingredientA) ||
            !inventory.HasItem(recipe.ingredientB))
        {
            Debug.Log("You don't have the required ingredients!");
            return false;
        }

        // Consume the ingredients.
        inventory.RemoveItem(recipe.ingredientA);
        inventory.RemoveItem(recipe.ingredientB);

        // Add the crafted item.
        inventory.AddItem(recipe.result);

        Debug.Log("Successfully crafted: " + recipe.result.itemName);

        // Play crafting audio.
        if (playerInteraction == null)
            playerInteraction = FindFirstObjectByType<PlayerInteraction>();

        playerInteraction?.PlayCraftingSound();

        // Update the inventory display.
        FindFirstObjectByType<InventoryUI>()?.RefreshInventory();

        return true;
    }
}
