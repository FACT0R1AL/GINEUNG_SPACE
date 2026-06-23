using UnityEngine.UI;
using UnityEngine;
using System.Text;

public class ItemInventorySlot : MonoBehaviour
{
    public Image itemIcon;
    public Text itemCountText;
    public Text itemName;
    
    public void SetItem(InventoryItem item)
    {
        StringBuilder itemName = new StringBuilder();
                switch (item.itemType)
                {
                    case ItemType.IronLv1:
                        itemName.Append("철 Lv.1");
                        break;
                    case ItemType.IronLv2:
                        itemName.Append("철 Lv.2");
                        break;
                    case ItemType.IronLv3:
                        itemName.Append("철 Lv.3");
                        break;
                    case ItemType.CopperLv1:
                        itemName.Append("구리 Lv.1");
                        break;
                    case ItemType.CopperLv2:
                        itemName.Append("구리 Lv.2");
                        break;
                    case ItemType.CopperLv3:
                        itemName.Append("구리 Lv.3");
                        break;
                    case ItemType.PlasticLv1:
                        itemName.Append("플라스틱 Lv.1");
                        break;
                    case ItemType.PlasticLv2:
                        itemName.Append("플라스틱 Lv.2");
                        break;
                    case ItemType.PlasticLv3:
                        itemName.Append("플라스틱 Lv.3");
                        break;
                    case ItemType.CoreLv1:
                        itemName.Append("코어 Lv.1");
                        break;
                    case ItemType.CoreLv2:
                        itemName.Append("코어 Lv.2");
                        break;
                    case ItemType.item1:
                        itemName.Append("우주선 워프");
                        break;
                    case ItemType.item2:
                        itemName.Append("우주선 이속증가");
                        break;
                    case ItemType.item3:
                        itemName.Append("우주선 스테미너");
                        break;
                    case ItemType.item4:
                        itemName.Append("보호막 생성");
                        break;
                }

        itemIcon.sprite = item.icon;
        itemCountText.text = item.count.ToString();
        this.itemName.text = itemName.ToString();
    }
}
