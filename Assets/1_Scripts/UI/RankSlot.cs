using UnityEngine;
using UnityEngine.UI;

public class RankSlot : MonoBehaviour
{
    public Text rankText;
    public Text nameText;
    public Text scoreText;

    public void Set(int rank, string name, int score)
    {
        rankText.text = rank.ToString();
        nameText.text = name;
        scoreText.text = score.ToString();
    }
}
