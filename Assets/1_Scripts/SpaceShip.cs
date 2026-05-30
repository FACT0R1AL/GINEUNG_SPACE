using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics; // Spline API 사용을 위해 필요합니다.

public class SpaceShip : MonoBehaviour
{
	[Header("Move")]
	public SplineContainer spline; // GameObject 대신 SplineContainer로 변경
	public float currentMoveSpeed;
	public float maxMoveSpeed = 4f;

	[Header("Hitbox")]
	public GameObject spaceshipHitbox;

	private float distancePercentage = 0f; // 0(시작점) ~ 1(끝점) 사이의 진행도
	private float splineLength;

	private void Start()
	{
		currentMoveSpeed = maxMoveSpeed;

		if (spline != null)
		{
			// 스플라인의 전체 길이를 구합니다.
			splineLength = spline.CalculateLength(0);
		}
	}

	private void Update()
	{
		if (spline == null || splineLength <= 0) return;

		// 1. 속도 감소 및 클램프 (기존 로직 유지)
		if (Input.GetKeyDown(KeyCode.F1))
		{
			currentMoveSpeed = maxMoveSpeed;
		}
		currentMoveSpeed -= 0.02f * Time.deltaTime; // 원하는 속도로 조절하세요. 
		currentMoveSpeed = Mathf.Clamp(currentMoveSpeed, 1f, maxMoveSpeed);

		// 2. 현재 속도를 바탕으로 진행도(Percentage)를 누적 계산합니다.
		// (이동거리 = 속도 * 시간)을 전체 길이로 나누어 0~1 사이의 비율로 만듭니다.
		distancePercentage += (currentMoveSpeed * Time.deltaTime) / splineLength;

		// 3. 계산된 진행도를 바탕으로 위치와 회전값을 가져와 우주선에 적용합니다.
		EvaluateSplinePosition(distancePercentage);
	}

	private void EvaluateSplinePosition(float t)
	{
		// 스플라인 상의 위치(Position)와 진행 방향(Tangent)을 계산합니다.
		float3 position;
		float3 tangent;
		float3 upVector;

		spline.Evaluate(0, t, out position, out tangent, out upVector);

		// 로컬 좌표를 월드 좌표로 변환하여 적용합니다.
		transform.position = spline.transform.TransformPoint(position);

		// 방향(Rotation) 설정 (Spline Animate의 Object Y+ Forward, Object Z+ Up 축 정렬 기준)
		if (!tangent.Equals(float3.zero))
		{
			// ForwardAxis가 Y+였으므로, 탄젠트(진행방향)를 Y축으로 설정합니다.
			Quaternion targetRotation = Quaternion.LookRotation(tangent, upVector);
			// 필요에 따라 축 회전 보정이 필요할 수 있습니다. 
			// 기본 방향이 안 맞으면 아래 축 정렬을 활성화해보세요.
			transform.rotation = spline.transform.rotation * targetRotation * Quaternion.Euler(90, 0, 0);
		}
	}
}