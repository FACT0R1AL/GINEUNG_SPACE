using UnityEngine;

public class Resource : MonoBehaviour
{
    public GameObject ResourceUI;

    [Header("INFO")]
    public ItemType itemType;
    public int count;

    [HideInInspector]
    public int poolTypeIndex;

    private void Update()
    {
        if (!ResourceUI.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            InventoryManager.Instance.AddItem(itemType, count);
            switch (itemType)
            {
                case ItemType.IronLv1:
                    GameManager.instance.messageUI.SendMessage("철lv.1을 획득하였습니다.", Color.white);
                    break;
                case ItemType.CopperLv1:
                    GameManager.instance.messageUI.SendMessage("구리lv.1을 획득하였습니다.", Color.white);
                    break;
                case ItemType.PlasticLv1:
                    GameManager.instance.messageUI.SendMessage("플라스틱lv.1을 획득하였습니다.", Color.white);
                    break;
                default:
                    GameManager.instance.messageUI.SendMessage($"{itemType.ToString()}을(를) 획득하였습니다.", Color.white);
                    break;
            }
            ResourceUI.SetActive(false);
            CreateResource.Instance.ReturnToPoolDelayed(gameObject);
        }
    }
}
