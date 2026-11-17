using UnityEngine;

public enum ItemEffectType
{
    None,
    Heal,
    Damage,
    ReduceDiseaseDamage,
    PauseDiseaseDamage,
    TriggerVisionSequence

}

public enum ItemCategory
{
    Ingredient, 
    Consumable, 
    Questitem
}

[CreateAssetMenu(
    fileName = "New Item",
    menuName = "RPG/Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Information")]
    public string itemName;

    [TextArea(2, 5)]
    public string description;

    [TextArea(1, 3)]
    public string effectDescription;

    [Header("Item Behavior")]
    public bool isUsable;
    public bool consumeOnUse = true;

    public ItemEffectType effectType;
    public float effectAmount;
    public float effectDuration;

    [Min(1)]
    public int maxStackSize = 99;

    [Header("Classification")]
    public ItemCategory category;
}