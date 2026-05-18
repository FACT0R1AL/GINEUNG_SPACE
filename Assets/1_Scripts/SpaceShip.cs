using System.Collections;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
	public GameObject pathObj;
	public float currentMoveSpeed;
	public float maxMoveSpeed;

	private LineRenderer lineRenderer;
	private Vector3[] pathPos;
	private int index;

	private void Start()
	{
		currentMoveSpeed = maxMoveSpeed;

		lineRenderer = pathObj.GetComponent<LineRenderer>();
		pathPos = pathObj.GetComponent<PathMaker>().pathPos;

		index = 0;
	}

	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, pathPos[index], currentMoveSpeed * Time.deltaTime);
	
		if (Vector3.Distance(transform.position, pathPos[index]) < 0.01f)
		{
			if (index == pathPos.Length-1)
			{
				index = 0;
				transform.position = pathPos[index];
			}

			index++;

			Vector3 dir = pathPos[index] - transform.position;
			Quaternion targetRotation = Quaternion.LookRotation(dir);
			targetRotation *= Quaternion.Euler(90f, 0f, 0f);

			StartCoroutine(SmoothRotate(transform.rotation, targetRotation));
		}

		if (Input.GetKeyDown(KeyCode.F))
		{
			currentMoveSpeed = maxMoveSpeed;
			
		}

		currentMoveSpeed -= 0.02f * Time.deltaTime;
		currentMoveSpeed = Mathf.Clamp(currentMoveSpeed, 1f, maxMoveSpeed);
	}

	IEnumerator SmoothRotate(Quaternion startRot, Quaternion endRot) 
	{
		float t = 0f;

		while (t <= 1f)
		{
			t += Time.deltaTime / 0.1f;

			transform.rotation = Quaternion.Lerp(startRot, endRot, t);

			yield return null;
		}
	}
}
