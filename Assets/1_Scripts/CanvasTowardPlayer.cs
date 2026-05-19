using UnityEngine;

public class CanvasTowardPlayer : MonoBehaviour
{
	public Canvas worldCanvas;
	private GameObject player;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");

		// 만약 인스펙터에서 worldCanvas를 깜빡하고 할당 안 했다면 자동으로 가져오기
		if (worldCanvas == null)
		{
			worldCanvas = GetComponent<Canvas>();
		}
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