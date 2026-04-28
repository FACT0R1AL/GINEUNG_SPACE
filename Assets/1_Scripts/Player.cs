using UnityEngine;

public class Player : MonoBehaviour
{
	public Vector3 currentVelocity;
	public float maxVelocity = 10f;
	public float moveSpeed = 5f;

	public float maxDistance = 250;

	private Camera mainCamera;
	private GameObject Spaceship;

	private LineRenderer lineRenderer;

	private void Start()
	{
		mainCamera = Camera.main;
		Spaceship = GameObject.FindGameObjectWithTag("Spaceship");

		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update()
	{
		// 이동
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");

		bool q = Input.GetKey(KeyCode.Q);
		bool e = Input.GetKey(KeyCode.E);

		Vector3 camForward = mainCamera.transform.forward;
		Vector3 camRight = mainCamera.transform.right;

		Vector3 moveDir = (camForward * v) + (camRight * h);

		currentVelocity += moveDir * Time.deltaTime * moveSpeed;

		if (q) currentVelocity -= mainCamera.transform.up * Time.deltaTime * moveSpeed;
		if (e) currentVelocity += mainCamera.transform.up * Time.deltaTime * moveSpeed;

		currentVelocity.x = Mathf.Clamp(currentVelocity.x, -maxVelocity, maxVelocity);
		currentVelocity.y = Mathf.Clamp(currentVelocity.y, -maxVelocity, maxVelocity);
		currentVelocity.z = Mathf.Clamp(currentVelocity.z, -maxVelocity, maxVelocity);

		transform.rotation = mainCamera.transform.rotation;

		transform.Translate(currentVelocity * Time.deltaTime, Space.World);


		// 최대 거리 (=산소 케이블)
		float distance = Vector3.Distance(transform.position, Spaceship.transform.position);
		Debug.Log(distance);

		if (distance > maxDistance)
		{ 
			transform.position = Spaceship.transform.position + new Vector3 (0f, 5f, 0f);
			currentVelocity = Vector3.zero;
		}

		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, Spaceship.transform.position);

		Ray ray = new Ray(transform.position, transform.forward * 5f);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, 5f, LayerMask.GetMask("Resources"))) {
			ShowResourceUI(hit);
		}
	}

	void ShowResourceUI(RaycastHit hit)
	{

	}
}