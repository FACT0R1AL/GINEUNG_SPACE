using System.Collections;
using UnityEngine;

public class SpaceShipMove : MonoBehaviour
{
	public GameObject pathObj;
	public float moveSpeed;

	private LineRenderer lineRenderer;
	private Vector3[] pathPos;

	private int index;

	private void Start()
	{
		lineRenderer = pathObj.GetComponent<LineRenderer>();
		pathPos = pathObj.GetComponent<PathMaker>().pathPos;
		index = 0;
	}

	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, pathPos[index], moveSpeed * Time.deltaTime);
	
		if (Vector3.Distance(transform.position, pathPos[index]) < 0.01f)
		{
			if (index == pathPos.Length-1)
			{
				index = 0;
				transform.position = pathPos[index];
			}
			index++;

			Debug.Log(index);
			Debug.Log(pathPos[index]);

			Vector3 dir = pathPos[index] - transform.position;
			Quaternion targetRotation = Quaternion.LookRotation(dir);
			targetRotation *= Quaternion.Euler(90f, 0f, 0f);

			StartCoroutine(SmoothRotate(transform.rotation, targetRotation));
		}
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
