using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ItemType
{
    IronLv1,
    IronLv2,
    IronLv3,
    CopperLv1,
    CopperLv2,
    CopperLv3,
    PlasticLv1,
    PlasticLv2,
    PlasticLv3,
}

[System.Serializable]
public struct InventoryItem
{
    public ItemType itemType;
    public int count;
    public Sprite icon;
    public Color color;
    [TextArea]
    public string itemInfo;
}


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<InventoryItem> inventoryItems = new List<InventoryItem>();

    public CraftSystem craftSystem;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            foreach (var type in ItemType.GetValues(typeof(ItemType)))
            {
                AddItem((ItemType)type, 1);
            }
            Debug.Log("아이템다 추가됨");
        }
    }

    public void AddItem(ItemType itemType, int count)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].itemType == itemType)
            {
                var item = inventoryItems[i];
                item.count += count;
                inventoryItems[i] = item;
                return;
            }
        }
    }


    public int GetItemCount(ItemType itemType)
    {

        foreach (InventoryItem item in inventoryItems)
        {
            if (item.itemType == itemType)
            {
                return item.count;
            }
        }
        return -1;
    }

    public Sprite GetItemIcon(ItemType itemType)
    {
        foreach (InventoryItem item in inventoryItems)
        {
            if (item.itemType == itemType)
            {
                return item.icon;
            }
        }
        return null;
    }
    
    public Color GetItemColor(ItemType itemType)
    {
        foreach (InventoryItem item in inventoryItems)
        {
            if (item.itemType == itemType)
            {
                return item.color;
            }
        }
        return Color.red;
    }

    public string GetItemInfo(ItemType itemType)
    {
        foreach (InventoryItem item in inventoryItems)
        {
            if (item.itemType == itemType)
            {
                return item.itemInfo;
            }
        }
        return "아이템 정보 없음";
    }
}
