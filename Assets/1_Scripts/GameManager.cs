using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject inventoryUI;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
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
