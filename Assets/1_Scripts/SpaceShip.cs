using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics; // Spline API ����� ���� �ʿ��մϴ�.

public class SpaceShip : MonoBehaviour
{
	[Header("Move")]
	public SplineContainer spline; // GameObject ��� SplineContainer�� ����
	public float currentMoveSpeed;
	public float maxMoveSpeed = 4f;

	[Header("Hitbox")]
	public GameObject spaceshipHitbox;

	private float distancePercentage = 0f; // 0(������) ~ 1(����) ������ ���൵
	private float splineLength;

	private void Start()
	{
		currentMoveSpeed = maxMoveSpeed;

		if (spline != null)
		{
			// ���ö����� ��ü ���̸� ���մϴ�.
			splineLength = spline.CalculateLength(0);
		}
	}

	private void Update()
	{
		if (spline == null || splineLength <= 0) return;

		// 1. �ӵ� ���� �� Ŭ���� (���� ���� ����)
		if (Input.GetKeyDown(KeyCode.F1))
		{
			currentMoveSpeed = maxMoveSpeed;
		}
		currentMoveSpeed -= 0.02f * Time.deltaTime; // ���ϴ� �ӵ��� �����ϼ���. 
		currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, maxMoveSpeed, 0.5f * Time.deltaTime); 

		distancePercentage += (currentMoveSpeed * Time.deltaTime) / splineLength;

		// 3. ���� ���൵�� �������� ��ġ�� ȸ������ ������ ���ּ��� �����մϴ�.
		EvaluateSplinePosition(distancePercentage);
	}

	private void EvaluateSplinePosition(float t)
	{
		// ���ö��� ���� ��ġ(Position)�� ���� ����(Tangent)�� ����մϴ�.
		float3 position;
		float3 tangent;
		float3 upVector;

		spline.Evaluate(0, t, out position, out tangent, out upVector);

		// ���� ��ǥ�� ���� ��ǥ�� ��ȯ�Ͽ� �����մϴ�.
		transform.position = spline.transform.TransformPoint(position);

		// ����(Rotation) ���� (Spline Animate�� Object Y+ Forward, Object Z+ Up �� ���� ����)
		if (!tangent.Equals(float3.zero))
		{
			// ForwardAxis�� Y+�����Ƿ�, ź��Ʈ(�������)�� Y������ �����մϴ�.
			Quaternion targetRotation = Quaternion.LookRotation(tangent, upVector);
			// �ʿ信 ���� �� ȸ�� ������ �ʿ��� �� �ֽ��ϴ�. 
			// �⺻ ������ �� ������ �Ʒ� �� ������ Ȱ��ȭ�غ�����.
			transform.rotation = spline.transform.rotation * targetRotation * Quaternion.Euler(90, 0, 0);
		}
	}

}