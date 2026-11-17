using UnityEngine;

public class CraftingUI : MonoBehaviour
{
    private CraftingSystem craftingSystem;

    private void Start()
    {
        craftingSystem = FindFirstObjectByType<CraftingSystem>();

        if (craftingSystem == null)
            Debug.LogError("CraftingSystem could not be found!");
    }

    public void CraftRecipe(CraftingRecipe recipe)
    {
        Debug.Log("CraftingUI received recipe: " + recipe.name);

        if (craftingSystem == null)
            craftingSystem = FindFirstObjectByType<CraftingSystem>();

        if (craftingSystem == null || recipe == null)
        {
            Debug.LogWarning("Crafting references are missing!");
            return;
        }

        craftingSystem.Craft(recipe);
    }
}