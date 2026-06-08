using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public struct CraftingRecipe
    {
        public ItemType itemType1;
        public ItemType itemType2;
        public ItemType upgradeItemType;
        public int itemType1Count;
        public int itemType2Count;
    }

public class CraftSystemItem : MonoBehaviour
{
    public Image ingredientsImage;
    public Image ingredientsImage2;

    public Image ingredientsImage_2;
    public Image ingredientsImage2_2;
    public Image resultImage;
    public Image resultImage2;
    public Text ingredientsCountText;
    public Text ingredientsCountText2;
    public Text resultCountText;
    public Text currentItemInfoText;
    public Text upgradeItemInfoText;
    public Button craftButton;
    public GameObject itemCraftParticle;

    public List<CraftingRecipe> craftingRecipes;
    private CraftingRecipe currentRecipe;

    private void Awake()
    {
        craftButton.onClick.AddListener(OnCraftButtonClick);
    }

	private void Start()
	{
		SetItem(currentRecipe.upgradeItemType);
	}

	private void Update()
    {
		int index = (int)currentRecipe.itemType1;
		ItemType upgradeItemType = (ItemType)(index + 1);
		ingredientsCountText.text = InventoryManager.Instance.GetItemCount(currentRecipe.itemType1).ToString();
		ingredientsCountText2.text = InventoryManager.Instance.GetItemCount(currentRecipe.itemType2).ToString();
		resultCountText.text = InventoryManager.Instance.GetItemCount(currentRecipe.upgradeItemType).ToString();
        currentItemInfoText.text =
            $"현재 아이템 정보\n\n" +
            $"{InventoryManager.Instance.GetItemInfo(currentRecipe.itemType1)}\n\n" +
            $"{InventoryManager.Instance.GetItemInfo(currentRecipe.itemType2)}";
        upgradeItemInfoText.text =
            $"업그레이드 아이템 정보\n\n" + 
            $"{InventoryManager.Instance.GetItemInfo(currentRecipe.upgradeItemType)}";
    }

    private void OnCraftButtonClick()
    {
        // Implement crafting logic here
        Debug.Log("Craft button clicked!");

        int count1 = InventoryManager.Instance.GetItemCount(currentRecipe.itemType1);
        int count2 = InventoryManager.Instance.GetItemCount(currentRecipe.itemType2);
        if (count1 >= currentRecipe.itemType1Count && count2 >= currentRecipe.itemType2Count)
        {
			count1 -= currentRecipe.itemType1Count;
			count2 -= currentRecipe.itemType2Count;

			int index = (int)currentRecipe.itemType1;
            ItemType upgradeItemType = (ItemType)(index + 1);
            bool core1 =false;
            bool core2 = false;
            if (currentRecipe.itemType1 == ItemType.CopperLv1 || currentRecipe.itemType1 == ItemType.IronLv1 || currentRecipe.itemType1 == ItemType.PlasticLv1)
            {
                core1 = true;
            }
            if (currentRecipe.itemType2 == ItemType.CopperLv2 || currentRecipe.itemType2 == ItemType.IronLv2 || currentRecipe.itemType2 == ItemType.PlasticLv2)
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
                if (InventoryManager.Instance.inventoryItems[i].itemType == currentRecipe.itemType1)
                {
                    var item = InventoryManager.Instance.inventoryItems[i];
                    item.count = count1;
                    InventoryManager.Instance.inventoryItems[i] = item;
                }
                if (InventoryManager.Instance.inventoryItems[i].itemType == currentRecipe.itemType2)
                {
                    var item = InventoryManager.Instance.inventoryItems[i];
                    item.count = count2;
                    InventoryManager.Instance.inventoryItems[i] = item;
                }
                if (InventoryManager.Instance.inventoryItems[i].itemType == currentRecipe.upgradeItemType)
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

    public void SetItem(ItemType upgradeItemType)
    {
        for (int i = 0; i < craftingRecipes.Count; i++)
        {
            if (craftingRecipes[i].upgradeItemType == upgradeItemType)
            {
                currentRecipe = craftingRecipes[i];
                break;
            }
        }
        ingredientsImage.color = InventoryManager.Instance.GetItemColor(currentRecipe.itemType1);
        ingredientsImage_2.color = InventoryManager.Instance.GetItemColor(currentRecipe.itemType2);
        resultImage.color = InventoryManager.Instance.GetItemColor(currentRecipe.upgradeItemType);

        ingredientsImage2.sprite = InventoryManager.Instance.GetItemIcon(currentRecipe.itemType1);
        ingredientsImage2_2.sprite = InventoryManager.Instance.GetItemIcon(currentRecipe.itemType2);
        resultImage2.sprite = InventoryManager.Instance.GetItemIcon(currentRecipe.upgradeItemType);

        ingredientsCountText.text = InventoryManager.Instance.GetItemCount(currentRecipe.itemType1).ToString();
        ingredientsCountText2.text = InventoryManager.Instance.GetItemCount(currentRecipe.itemType2).ToString();
        resultCountText.text = InventoryManager.Instance.GetItemCount(currentRecipe.upgradeItemType).ToString();
        

    }
}
