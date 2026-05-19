using UnityEngine;


[System.Serializable]
public struct SpriteInfo
{
	public Sprite sprite;
	public ItemType itemType;
}

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public GameObject inventoryUI;
	public SpriteInfo[] spriteInfos;


    private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			if (inventoryUI.activeSelf == false)
			{
				inventoryUI.SetActive(true);
			}
			else
			{
				inventoryUI.SetActive(false);
			}
		}
	}

	public Sprite GetSprite(ItemType itemType)
	{
		foreach (var info in spriteInfos)
		{
			if (info.itemType == itemType)
			{
				return info.sprite;
			}
		}
		return null; // 또는 기본 스프라이트 반환
    }
}
