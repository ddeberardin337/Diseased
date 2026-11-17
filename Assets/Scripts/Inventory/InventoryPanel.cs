
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    [Header("Content References")]
    [SerializeField] private GameObject inventoryContent;
    [SerializeField] private GameObject craftingContent;
    [SerializeField] private GameObject itemDetailsContent;

    private void Start()
    {
        ShowInventory();
    }

    public void ShowInventory()
    {
        inventoryContent.SetActive(true);
        craftingContent.SetActive(false);
        itemDetailsContent.SetActive(false);
    }

    public void ShowCrafting()
    {
        inventoryContent.SetActive(false);
        craftingContent.SetActive(true);
        itemDetailsContent.SetActive(false);
    }

    public void ShowItemDetails()
    {
        inventoryContent.SetActive(false);
        craftingContent.SetActive(false);
        itemDetailsContent.SetActive(true);
    }
}
