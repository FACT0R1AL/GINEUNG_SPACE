using UnityEngine;

public class CanvasTowardPlayer : MonoBehaviour
{
    public Canvas worldCanvas;
	public GameObject player;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	private void Update()
	{
		worldCanvas.transform.LookAt(player.transform.position);

		Vector3 rot = worldCanvas.transform.eulerAngles;
		worldCanvas.transform.rotation = Quaternion.Euler(rot.x, rot.y + 180f, rot.z + 180f);
	}
}
