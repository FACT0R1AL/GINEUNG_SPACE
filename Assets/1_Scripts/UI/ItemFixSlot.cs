using UnityEngine;
using UnityEngine.UI;

public class ItemFixSlot : MonoBehaviour
{
    public Image icon;
    public Text levelAndCount;

    public void SetItem(Sprite itemIcon, int level, int count)
    {
        icon.sprite = itemIcon;
        levelAndCount.text = $"Lv.{level} X{count}";
    }
}
