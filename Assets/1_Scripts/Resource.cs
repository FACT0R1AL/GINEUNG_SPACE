using UnityEngine;

public class Resource : MonoBehaviour
{
	public GameObject ResourceUI;

	[Header("INFO")]
    public ItemType itemType;
    public int count;

	private void Update()
	{
		if (ResourceUI.activeSelf == true)
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				InventoryManager.Instance.AddItem(itemType, count);
				Destroy(gameObject);
			}
		}
	}
}
