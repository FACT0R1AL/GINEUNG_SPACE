using UnityEngine;
using UnityEngine.UI;

public class CraftSystem : MonoBehaviour
{
    public Image ingredientsImage;
    public Image ingredientsImage2;
    public Image resultImage;
    public Image resultImage2;
    public Text ingredientsCountText;
    public Text resultCountText;
    public Text currentItemInfoText;
    public Text upgradeItemInfoText;
    public Button craftButton;
    public GameObject itemCraftParticle;

    [SerializeField] private ItemType currentItemType;

    private void Awake()
    {
        craftButton.onClick.AddListener(OnCraftButtonClick);
    }

	private void Start()
	{
		int index = (int)currentItemType;
		ItemType upgradeItemType = (ItemType)(index + 1);
		SetItem(currentItemType, upgradeItemType);
	}

	private void Update()
    {
		int index = (int)currentItemType;
		ItemType upgradeItemType = (ItemType)(index + 1);
		ingredientsCountText.text = InventoryManager.Instance.GetItemCount(currentItemType).ToString();
		resultCountText.text = InventoryManager.Instance.GetItemCount(upgradeItemType).ToString();
        currentItemInfoText.text =
            $"현재 아이템 정보\n\n" +
            $"{InventoryManager.Instance.GetItemInfo(currentItemType)}";
        upgradeItemInfoText.text =
            "업그레이드 아이템 정보\n\n" + 
            $"{InventoryManager.Instance.GetItemInfo(upgradeItemType)}";
    }

    private void OnCraftButtonClick()
    {
        // Implement crafting logic here
        Debug.Log("Craft button clicked!");

        int count = InventoryManager.Instance.GetItemCount(currentItemType);
        if (count >= 2)
        {
			count -= 2;

			int index = (int)currentItemType;
            ItemType upgradeItemType = (ItemType)(index + 1);

            for (int i = 0; i < InventoryManager.Instance.inventoryItems.Count; i++)
            {
                if (InventoryManager.Instance.inventoryItems[i].itemType == currentItemType)
                {
                    var item = InventoryManager.Instance.inventoryItems[i];
                    item.count = count;
                    InventoryManager.Instance.inventoryItems[i] = item;
                }
                if (InventoryManager.Instance.inventoryItems[i].itemType == upgradeItemType)
                {
                    var item = InventoryManager.Instance.inventoryItems[i];
                    item.count++;
                    InventoryManager.Instance.inventoryItems[i] = item;
                }
            }

            var particle = Instantiate(itemCraftParticle, transform);
            var itemShowParticleUI = particle.GetComponent<ItemShowParticleUI>();
            itemShowParticleUI.ShowItem(new InventoryItem
            {
                itemType = upgradeItemType,
                icon = InventoryManager.Instance.GetItemIcon(upgradeItemType),
                color = InventoryManager.Instance.GetItemColor(upgradeItemType)
            });
            Destroy(particle, 2f);
        }
    }

    public void SetItem(ItemType itemType, ItemType upgradeItemType)
    {
        ingredientsImage.color = InventoryManager.Instance.GetItemColor(itemType);
        resultImage.color = InventoryManager.Instance.GetItemColor(upgradeItemType);

        ingredientsImage2.sprite = InventoryManager.Instance.GetItemIcon(itemType);
        resultImage2.sprite = InventoryManager.Instance.GetItemIcon(upgradeItemType);

        ingredientsCountText.text = InventoryManager.Instance.GetItemCount(itemType).ToString();
        resultCountText.text = InventoryManager.Instance.GetItemCount(upgradeItemType).ToString();
        
        currentItemType = itemType;
    }
}
