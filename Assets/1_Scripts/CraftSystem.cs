using UnityEditor;
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
            $"???? ?????? ????\n\n" +
            $"{InventoryManager.Instance.GetItemInfo(currentItemType)}";
        upgradeItemInfoText.text =
            "?????????? ?????? ????\n\n" + 
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
            bool core1 =false;
            bool core2 = false;
            if (currentItemType == ItemType.CopperLv1 || currentItemType == ItemType.IronLv1 || currentItemType == ItemType.PlasticLv1)
            {
                core1 = true;
            }
            if (currentItemType == ItemType.CopperLv2 || currentItemType == ItemType.IronLv2 || currentItemType == ItemType.PlasticLv2)
            {
                core2 = true;
            }
            bool craft = false;
            int randomValue = Random.Range(0, 101);
            if (randomValue <= 50)
            {
                craft = true;
            }

            ItemType plusItemType = ItemType.CoreLv1;
            if (core1)
            {
                plusItemType = ItemType.CoreLv1;
            }

            if (core2)
            {
                plusItemType = ItemType.CoreLv2;
            }

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

                if (craft)
                {
                    if (InventoryManager.Instance.inventoryItems[i].itemType == plusItemType)
                    {
                        var item = InventoryManager.Instance.inventoryItems[i];
                        item.count++;
                        InventoryManager.Instance.inventoryItems[i] = item;
                    }
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
