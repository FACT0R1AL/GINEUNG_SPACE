using UnityEngine;

public class Player : MonoBehaviour
{
	[Header("Move")]
	public Vector3 currentVelocity;
	public float maxVelocity = 10f;
	public float moveSpeed = 5f;
	public float maxDistance = 250;

	[Header("Get")]
	public float maxGetDistance;

	[Header("Oxygen")]
	public float currentOxygen;
	public float maxOxygen;

	private Camera mainCamera;

	private GameObject Spaceship;
	public bool isInSpaceship;

	private GameObject currentResource;

	private LineRenderer lineRenderer;

	public Rigidbody rb;

	private void Start()
	{
		currentOxygen = maxOxygen;

		mainCamera = Camera.main;
		Spaceship = GameObject.FindGameObjectWithTag("Spaceship");

		lineRenderer = GetComponent<LineRenderer>();
		rb = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		// �̵�
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


		// �ִ� �Ÿ�
		float distance = Vector3.Distance(transform.position, Spaceship.transform.position);

		if (distance > maxDistance)
		{ 
			transform.position = Spaceship.transform.position + new Vector3 (0f, 5f, 0f);
			currentVelocity = Vector3.zero;
		}

		// �ڿ��Ա�
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, maxGetDistance, LayerMask.GetMask("Resource")))
		{
			Debug.Log(hit.collider.name);

			if (currentResource != hit.collider.gameObject)
			{
				if (currentResource != null)
					currentResource.transform.GetChild(0).gameObject.SetActive(false);

				currentResource = hit.collider.gameObject;
				currentResource.transform.GetChild(0).gameObject.SetActive(true);
			}
		}
		else
		{
			if (currentResource != null)
			{
				currentResource.transform.GetChild(0).gameObject.SetActive(false);
				currentResource = null;
			}
		}

		// ���
		if (Input.GetKeyDown(KeyCode.F1))
		{
			currentOxygen = maxOxygen;
		}

		if (isInSpaceship)
		{
			currentOxygen += 8f * Time.deltaTime;
		}
		else
		{
			currentOxygen -= 0.5f * Time.deltaTime;
		}
		currentOxygen = Mathf.Clamp(currentOxygen, 0, maxOxygen);

		if (isInSpaceship)
		{
			if (Input.GetKeyDown(KeyCode.F))
			{
				if (!GameManager.instance.inSpaceShipUI.activeSelf)
				{
					GameManager.instance.inSpaceShipUI.SetActive(true);
					currentVelocity = Vector3.zero;
					rb.isKinematic = true;
					transform.parent = Spaceship.transform;
				}
				else
				{
					GameManager.instance.inSpaceShipUI.SetActive(false);
					rb.isKinematic = false;
					transform.parent = null;
				}
			}
		}
	}

	

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("SpaceshipHitbox"))
		{
			isInSpaceship = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.CompareTag("SpaceshipHitbox"))
		{
			isInSpaceship = false;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Resource"))
		{
			Vector3 dir = transform.position - collision.transform.position;
			collision.gameObject.GetComponent<Rigidbody>().AddForce(dir.normalized * currentVelocity.magnitude, ForceMode.Impulse);
		}
	}
}