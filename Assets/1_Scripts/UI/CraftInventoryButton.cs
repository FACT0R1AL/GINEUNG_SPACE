using UnityEngine;
using UnityEngine.UI;

public class CraftInventoryButton : MonoBehaviour
{
    public Button button;
    public Text countText;
    public ItemType itemType;

    private void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    private void OnButtonClick()
    {
        int count = InventoryManager.Instance.GetItemCount(itemType);
        int index = (int)itemType;
        ItemType upgradeItemType = (ItemType)(index + 1);
        Debug.Log($"{itemType.ToString()} | {upgradeItemType.ToString()}");
        InventoryManager.Instance.craftSystem.setItem(itemType, upgradeItemType);
    }
}
