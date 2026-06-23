using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform inventorySlotParent;
    public GameObject inventorySlotPrefab;

    public GameObject inventoryUI;

    public Slider sliderWeight;
    public Image weightIcon;
    public Text weightText;

    private void Start()
    {
        inventoryUI.SetActive(false);
    }

    public void Show()
    {
        inventoryUI.SetActive(true);
        UpdateInventoryUI();
    }
    

    void UpdateInventoryUI()
    {
        // 1. ���ϴ� UI ����
        foreach (Transform child in inventorySlotParent)
        {
            Destroy(child.gameObject);
        }
        float currentWeight = 0f;
        // 2. InventoryManager�� ���� ��Ʈ�� UI ����
        foreach (var item in InventoryManager.Instance.inventoryItems)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventorySlotParent);
            ItemInventorySlot slotScript = slot.GetComponent<ItemInventorySlot>();
            slotScript.SetItem(item);
            currentWeight += item.weight * item.count;
        }
        sliderWeight.value = currentWeight / InventoryManager.Instance.maxWeight;
        weightText.text = $"{currentWeight:F0} / {InventoryManager.Instance.maxWeight:F0}";
    }
}
