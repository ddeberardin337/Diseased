
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Crafting Recipe",
    menuName = "RPG/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Header("Ingredients")]
    public ItemData ingredientA;
    public ItemData ingredientB;

    [Header("Result")]
    public ItemData result;
}
