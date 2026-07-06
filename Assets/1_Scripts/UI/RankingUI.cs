using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RankingUI : MonoBehaviour
{
    public Transform rankPanel;
    public GameObject rankPrefab;

    public int maxPool = 10;
    private List<GameObject> ranks = new List<GameObject>();
    private List<GameObject> rankSlots = new List<GameObject>();

    public void Start()
    {
        for (int i = 0; i < maxPool; i++)
        {
            GameObject rankSlot = Instantiate(rankPrefab, rankPanel);
            ranks.Add(rankSlot);
            rankSlot.SetActive(false);
        }
        Hide();
    }

    public void RankShow()
    {
        gameObject.SetActive(true);
        RankDataList data = RankManager.Instance.rankList;
        data.ranks = data.ranks.OrderByDescending(x => x.score).ToList();
        foreach (RankData rank in data.ranks)
        {
            for (int i = 0; i < ranks.Count; i++)
            {
                if (!ranks[i].activeInHierarchy)
                {
                    ranks[i].SetActive(true);
                    RankSlot slot = ranks[i].GetComponent<RankSlot>();
                    slot.Set(data.ranks.IndexOf(rank) + 1, rank.name, rank.score);
                    rankSlots.Add(ranks[i]);
                    ranks.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public void Hide()
    {
        if (ranks.Count > 0)
        {
            foreach (var slot in rankSlots)
            {
                slot.SetActive(false);
                ranks.Add(slot);
            }
            rankSlots.Clear();
        }
        gameObject.SetActive(false);
    }
}
