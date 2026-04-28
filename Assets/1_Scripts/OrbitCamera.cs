using UnityEngine;

public class OrbitCamera : MonoBehaviour
{
	public Transform target;      // 카메라가 바라볼 대상
	public float distance = 5.0f; // 대상과의 거리
	public float xSpeed = 120.0f; // X축 회전 속도
	public float ySpeed = 120.0f; // Y축 회전 속도

	public float yMinLimit = -40f; // 아래쪽 각도 제한
	public float yMaxLimit = 80f;  // 위쪽 각도 제한

	public float distanceMin = 2f; // 최소 줌 거리
	public float distanceMax = 15f;// 최대 줌 거리

	float x = 0.0f;
	float y = 0.0f;

	void Start()
	{
		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;

		// 시작 시 타겟이 있다면 위치 초기화
		if (target != null)
		{
			UpdateCamera();
		}
	}

	void LateUpdate()
	{
		if (target)
		{
			// 마우스 오른쪽 버튼을 누를 때만 회전 (원하는 버튼으로 수정 가능)
			if (Input.GetMouseButton(1))
			{
				x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
				y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

				y = ClampAngle(y, yMinLimit, yMaxLimit);
			}

			// 마우스 휠로 줌인/아웃
			distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

			UpdateCamera();
		}
	}

	void UpdateCamera()
	{
		Quaternion rotation = Quaternion.Euler(y, x, 0);
		Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
		Vector3 position = rotation * negDistance + target.position;

		transform.rotation = rotation;
		transform.position = position;
	}

	// 각도 제한 함수
	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360F) angle += 360F;
		if (angle > 360F) angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}