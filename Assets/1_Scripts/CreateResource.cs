using UnityEngine;

public class CreateResource : MonoBehaviour
{
	public GameObject[] resourcePrefabs;
    public int maxResourceCount;
    public int currentResourceCount;

    private GameObject spaceship;
    private GameObject player;

	private float maxDistance;

	private void Start()
	{
        spaceship = GameObject.FindGameObjectWithTag("Spaceship");
        player = GameObject.FindGameObjectWithTag("Player");

		maxDistance = player.GetComponent<Player>().maxDistance;
	}

	private void Update()
	{
		while (currentResourceCount < maxResourceCount)
		{
			int randomIdx = Random.Range(0 ,resourcePrefabs.Length);

		}
	}
}
