using UnityEngine;
using UnityEngine.UI;

public class SystemUI : MonoBehaviour
{
    public Text nameAndLevelText;
    public Text currentDesc;
    public Text upgradeDesc;
    public Text upgradeItemsDesc;
    public Text upgradeButtonText;

    public Image[] stars = new Image[3];

    public Button upgradeButton;

    private void Awake()
    {
        upgradeButton.onClick.AddListener(Upgrade);
    }

    public void Start()
    {
        Set();
    }

    public void Upgrade()
    {
        bool canUpgrade = true;
        foreach (var itemData in SystemManager.Instance.engineSystem.upgradeInfos[SystemManager.Instance.engineSystem.currentLevel].upgradeData.itemDatas)
        {
            int count = InventoryManager.Instance.GetItemCount(itemData.itemType);
            if (count <= 0)
            {
                canUpgrade = false;
                break;
            }
            else
            {
                if (count < itemData.count)
                {
                    canUpgrade = false;
                    break;
                }
            }
        }
        if (canUpgrade)
        {

            foreach (var itemData in SystemManager.Instance.engineSystem.upgradeInfos[SystemManager.Instance.engineSystem.currentLevel].upgradeData.itemDatas)
            {
                for (int i = 0; i < InventoryManager.Instance.inventoryItems.Count; i++)
                {
                    if (InventoryManager.Instance.inventoryItems[i].itemType == itemData.itemType)
                    {
                        var item = InventoryManager.Instance.inventoryItems[i];
                        item.count -= itemData.count;
                        InventoryManager.Instance.inventoryItems[i] = item;
                    }
                }
            }
            SystemManager.Instance.engineSystem.currentLevel++;
            Set();
        }
    }

    public void Set()
    {
        nameAndLevelText.text = $"엔진 Lv.{SystemManager.Instance.engineSystem.currentLevel}";
        currentDesc.text = $"{SystemManager.Instance.engineSystem.upgradeInfos[SystemManager.Instance.engineSystem.currentLevel - 1].upgradeDesc}";

        if (SystemManager.Instance.engineSystem.currentLevel != 3)
        {
            upgradeDesc.text = $"{SystemManager.Instance.engineSystem.upgradeInfos[SystemManager.Instance.engineSystem.currentLevel].upgradeDesc}";
            upgradeItemsDesc.text = "";
            foreach (var itemData in SystemManager.Instance.engineSystem.upgradeInfos[SystemManager.Instance.engineSystem.currentLevel].upgradeData.itemDatas)
            {
                string itemName = "";
                switch (itemData.itemType)
                {
                    case ItemType.IronLv1:
                        itemName = "철 Lv.1";
                        break;
                    case ItemType.IronLv2:
                        itemName = "철 Lv.2";
                        break;
                    case ItemType.IronLv3:
                        itemName = "철 Lv.3";
                        break;
                    case ItemType.CopperLv1:
                        itemName = "구리 Lv.1";
                        break;
                    case ItemType.CopperLv2:
                        itemName = "구리 Lv.2";
                        break;
                    case ItemType.CopperLv3:
                        itemName = "구리 Lv.3";
                        break;
                    case ItemType.PlasticLv1:
                        itemName = "플라스틱 Lv.1";
                        break;
                    case ItemType.PlasticLv2:
                        itemName = "플라스틱 Lv.2";
                        break;
                    case ItemType.PlasticLv3:
                        itemName = "플라스틱 Lv.3";
                        break;
                }
                upgradeItemsDesc.text += $"{itemName} ({itemData.count})\n";
            }
            upgradeButton.interactable = true;
            upgradeButtonText.text = "업그레이드";
        }
        else
        {
            upgradeItemsDesc.text = "";
            upgradeDesc.text = "최대 레벨입니다";
            upgradeButton.interactable = false;
            upgradeButtonText.text = "최대 레벨";
        }

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < SystemManager.Instance.engineSystem.currentLevel; i++)
        {
            stars[i].gameObject.SetActive(true);
        }
    }
}
