using UnityEngine;
using UnityEngine.UI;

public class CraftSystem : MonoBehaviour
{
    public Image ingredientsImage;
    public Image ingredientsImage2;
    public Image resultImage;
    public Image resultImage2;
    public Text ingredientsCountText;
    public Text resultCountText;
    public Button craftButton;

    private void Awake()
    {
        craftButton.onClick.AddListener(OnCraftButtonClick);
    }

    private void OnCraftButtonClick()
    {
        // Implement crafting logic here
        Debug.Log("Craft button clicked!");
    }

    public void setItem(ItemType itemType, ItemType upgradeItemType)
    {
        ingredientsImage.color = InventoryManager.Instance.GetItemColor(itemType);
        resultImage.color = InventoryManager.Instance.GetItemColor(upgradeItemType);
        ingredientsImage2.sprite = InventoryManager.Instance.GetItemIcon(itemType);
        resultImage2.sprite = InventoryManager.Instance.GetItemIcon(upgradeItemType);
        ingredientsCountText.text = InventoryManager.Instance.GetItemCount(itemType).ToString();
        resultCountText.text = InventoryManager.Instance.GetItemCount(upgradeItemType).ToString();
    }
}
