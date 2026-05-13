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

    private void Update()
    {
        countText.text = InventoryManager.Instance.GetItemCount(itemType).ToString();
    }

    private void OnButtonClick()
    {
        if(itemType == ItemType.PlasticLv3)
        {
            Debug.Log("최고 레벨 아이템입니다.");
            return;
        }else if (itemType == ItemType.IronLv3)
        {
            return;
        }else if (itemType == ItemType.CopperLv3)
        {
            return;
        }
            int count = InventoryManager.Instance.GetItemCount(itemType);
        int index = (int)itemType;
        ItemType upgradeItemType = (ItemType)(index + 1);
        Debug.Log($"{itemType.ToString()} | {upgradeItemType.ToString()}");
        InventoryManager.Instance.craftSystem.setItem(itemType, upgradeItemType);
    }
}
