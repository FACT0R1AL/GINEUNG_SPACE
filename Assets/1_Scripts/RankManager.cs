using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

[Serializable]
public class RankDataList
{
    public List<RankData> ranks = new List<RankData>();
}

[Serializable]
public class RankData
{
    public string name;
    public int score;
}

public class RankManager : MonoBehaviour
{
    public static RankManager Instance { get; private set; }

    public RankDataList rankList;

    public string fileName = "RankData.json";
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Start()
    {
        Load();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TestAddRank();
            Save();
        }
    }

    public void TestAddRank()
    {
        RankData rankData = new RankData();
        rankData.score = UnityEngine.Random.Range(0, 10000);
        Debug.Log(rankData.score);
        rankList.ranks.Add(rankData);
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(rankList);
        string path =  Application.persistentDataPath + "/" + fileName;
        File.WriteAllText(path, json);
    }

    public void Load()
    {
        string path =  Application.persistentDataPath + "/" + fileName;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            rankList = JsonUtility.FromJson<RankDataList>(json);
        }
    }
}
