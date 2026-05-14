using UnityEngine;
using UnityEngine.UI;

public class SystemUI : MonoBehaviour
{
    public Text nameAndLevelText;
    public Text currentDesc;
    public Text upgradeDesc;
    public Text upgradeItesDesc;
    public Text upgradeButtonText;

    public Image[] starts = new Image[3];

    public Button upgradeButton;

    private void Awake()
    {
        upgradeButton.onClick.AddListener(Upgrade);
    }


    public void OnEnable()
    {
        Set();
    }
    public void Upgrade()
    {
        bool canUpgrade = true;
        foreach (var dd in SystemManager.Instance.engineSystem.upgradeInfos[SystemManager.Instance.engineSystem.currentLevel].upgradeData.itemDatas)
        {
            int count = InventoryManager.Instance.GetItemCount(dd.itemType);
            if (count <= 0)
            {
                canUpgrade = false;
                break;
            }
            else
            {
                if (count < dd.count)
                {
                    canUpgrade = false;
                    break;
                }
            }
        }
        if (canUpgrade)
        {

            foreach (var dd in SystemManager.Instance.engineSystem.upgradeInfos[SystemManager.Instance.engineSystem.currentLevel].upgradeData.itemDatas)
            {
                for (int i = 0; i < InventoryManager.Instance.inventoryItems.Count; i++)
                {
                    if (InventoryManager.Instance.inventoryItems[i].itemType == dd.itemType)
                    {
                        var item = InventoryManager.Instance.inventoryItems[i];
                        item.count -= dd.count;
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
        currentDesc.text = $"효과: {SystemManager.Instance.engineSystem.upgradeInfos[SystemManager.Instance.engineSystem.currentLevel - 1].upgradedesc}";
        if (SystemManager.Instance.engineSystem.currentLevel != 3)
        {
            upgradeDesc.text = $"효과: {SystemManager.Instance.engineSystem.upgradeInfos[SystemManager.Instance.engineSystem.currentLevel].upgradedesc}";
            upgradeItesDesc.text = "업그레이드 재료: ";
            foreach (var dd in SystemManager.Instance.engineSystem.upgradeInfos[SystemManager.Instance.engineSystem.currentLevel].upgradeData.itemDatas)
            {
                string itemName = "";
                switch (dd.itemType)
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
                upgradeItesDesc.text += $"{itemName} {dd.count}개 ";
            }
            upgradeButton.interactable = true;
            upgradeButtonText.text = "업그레이드";
        }
        else
        {
            upgradeItesDesc.text = "최대 레벨입니다.";
            upgradeDesc.text = "최대 레벨입니다.";
            upgradeButton.interactable = false;
            upgradeButtonText.text = "최대 레벨";
        }

        for (int i = 0; i < starts.Length; i++)
        {
            starts[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < SystemManager.Instance.engineSystem.currentLevel; i++)
        {
            starts[i].gameObject.SetActive(true);
        }
    }
}
