using UnityEngine;

public class PathMaker : MonoBehaviour
{
	public Transform startPoint;   // P0
	public Transform controlPoint; // P1 (곡선의 정점 방향)
	public Transform endPoint;     // P2

	public int segmentCount = 20;  // 곡선을 얼마나 부드럽게 할 것인가 (점의 개수)
	public Vector3[] pathPos;

	private LineRenderer lineRenderer;

	private void Start()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.positionCount = segmentCount + 1;
		pathPos = new Vector3[lineRenderer.positionCount];
	}

	void Update()
	{
		DrawBezierCurve();
	}

	void DrawBezierCurve()
	{
		for (int i = 0; i <= segmentCount; i++)
		{
			float t = i / (float)segmentCount;
			// 2차 베지에 공식: (1-t)^2*P0 + 2(1-t)t*P1 + t^2*P2
			Vector3 position = CalculateBezierPoint(t, startPoint.position, controlPoint.position, endPoint.position);
			lineRenderer.SetPosition(i, position);
			pathPos[i] = position;
		}
	}

	Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
	{
		// 수학적으로 부드러운 경로를 계산하는 공식
		return Mathf.Pow(1 - t, 2) * p0 + 2 * (1 - t) * t * p1 + Mathf.Pow(t, 2) * p2;
	}
}
