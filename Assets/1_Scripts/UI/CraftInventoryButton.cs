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
        if(itemType == ItemType.PlasticLv1)
        {
            Debug.Log("�ְ� ���� �������Դϴ�.");
            return;
        }
        else if (itemType == ItemType.IronLv1)
        {
			Debug.Log("�ְ� ���� �������Դϴ�.");
			return;
        }
        else if (itemType == ItemType.CopperLv1)
        {
			Debug.Log("�ְ� ���� �������Դϴ�.");
			return;
        }

        if (itemType == ItemType.CoreLv2)
        {
            Debug.Log("������ �Ұ����� �������Դϴ�");
            return;
        }

        if (itemType == ItemType.CoreLv1)
        {
            Debug.Log("������ �Ұ����� �������Դϴ�");
            return;
        }
        
        int count = InventoryManager.Instance.GetItemCount(itemType);
        int index = (int)itemType;
        ItemType upgradeItemType = (ItemType)(index -1);
        Debug.Log($"{itemType.ToString()} | {upgradeItemType.ToString()}");
        if (itemType == ItemType.item1 || itemType == ItemType.item2 || itemType == ItemType.item3 || itemType == ItemType.item4)
        {
            InventoryManager.Instance.craftSystem.gameObject.SetActive(false);
            InventoryManager.Instance.craftSystemItem.gameObject.SetActive(true);
            InventoryManager.Instance.craftSystemItem.SetItem(itemType);
            return;
        }
        InventoryManager.Instance.craftSystem.gameObject.SetActive(true);
            InventoryManager.Instance.craftSystemItem.gameObject.SetActive(false);
        InventoryManager.Instance.craftSystem.SetItem(upgradeItemType, itemType);
    }
}
