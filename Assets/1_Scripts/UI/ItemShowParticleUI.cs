using UnityEngine;
using UnityEngine.UI;

public class ItemShowParticleUI : MonoBehaviour
{
    public Image background;
    public Image icon;

    public void ShowItem(InventoryItem item)
    {
        icon.sprite = item.icon;
        background.color = item.color;
        gameObject.SetActive(true);
    }
}
