using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public GameObject inventoryUI;

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
		if (Input.GetKeyDown(KeyCode.F))
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
}
