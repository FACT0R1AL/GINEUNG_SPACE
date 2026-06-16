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
	public MessageUI messageUI;
	public GameObject inSpaceShipUI;
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

	private void Start()
	{
		inSpaceShipUI.SetActive(false);
	}

	private void Update()
	{
		// if (Input.GetKeyDown(KeyCode.Alpha1))
		// {
		// 	if (inventoryUI.activeSelf == false)
		// 	{
		// 		inventoryUI.SetActive(true);
		// 	}
		// 	else
		// 	{
		// 		inventoryUI.SetActive(false);
		// 	}
		// }
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
		return null; // �Ǵ� �⺻ ��������Ʈ ��ȯ
    }

	public void SendMessage(string message)
	{
		messageUI.SendMessage(message);
	}

	public void SendMessage(string message, Color color)
	{
		messageUI.SendMessage(message, color);
	}
}
