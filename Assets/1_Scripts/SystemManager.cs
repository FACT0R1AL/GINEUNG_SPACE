using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SystemData
{
    public int currentLevel;
    public int MaxLevel;
    public List<UpgradeInfo> upgradeInfos;
}

[System.Serializable]
public struct UpgradeInfo
{
    public int currentLevel;
    [TextArea]
    public string upgradeDesc;
    public UpgradeData upgradeData;
}

[System.Serializable]
public struct UpgradeData
{
    public List<ItemData> itemDatas;
}

[System.Serializable]
public struct ItemData
{
    public ItemType itemType;
    public int count;
}

public class SystemManager : MonoBehaviour
{
    public static SystemManager Instance { get; private set; }

    public SystemData engineSystem;

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
}
