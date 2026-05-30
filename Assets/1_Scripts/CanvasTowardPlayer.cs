using UnityEngine;

public class CanvasTowardPlayer : MonoBehaviour
{
	public Canvas worldCanvas;
	private GameObject player;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");

		if (worldCanvas == null)
		{
			worldCanvas = GetComponent<Canvas>();
		}

		worldCanvas.gameObject.SetActive(false);
	}

	private void Update()
	{
		// 예외 처리 (플레이어나 캔버스가 없으면 실행 안 함)
		if (player == null || worldCanvas == null) return;

		Vector3 targetDirection = worldCanvas.transform.position - player.transform.position;

		if (targetDirection != Vector3.zero)
		{
			worldCanvas.transform.rotation = Quaternion.LookRotation(targetDirection);
		}
	}
}