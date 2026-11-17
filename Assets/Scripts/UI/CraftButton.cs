using UnityEngine;
using UnityEngine.UI;

public class CraftButton : MonoBehaviour
{
    [SerializeField] private CraftingRecipe recipe;

    private CraftingUI craftingUI;
    private Button button;

    private void Start()
    {
        craftingUI = FindFirstObjectByType<CraftingUI>();
        button = GetComponent<Button>();

        if (craftingUI == null)
        {
            Debug.LogError("CraftingUI could not be found!");
            return;
        }

        if (button == null)
        {
            Debug.LogError("Button component could not be found!", this);
            return;
        }

        if (recipe == null)
        {
            Debug.LogError("No CraftingRecipe assigned to this button!", this);
            return;
        }

        button.onClick.AddListener(() =>
        {
            Debug.Log("Craft button clicked! Recipe: " + recipe.name);

            craftingUI.CraftRecipe(recipe);
        });
    }
}