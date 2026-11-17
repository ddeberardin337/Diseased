using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "RPG/Quest")]
public class QuestData : ScriptableObject
{
    [Header("Quest Items")]
    public ItemData[] requiredItems;
    
}
