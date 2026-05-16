using UnityEngine;

public class MinimapObject : MonoBehaviour
{
	public GameObject obj;
	public Vector3 pos;

	private void Update()
	{
		transform.position = obj.transform.position + pos;

		transform.rotation = Quaternion.Euler(90f, obj.transform.rotation.eulerAngles.y, 0f);
	}
}
