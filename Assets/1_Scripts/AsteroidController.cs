using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float impactDistance = 5f;
    public int debrisCount = 12;
    public float debrisSpreadRadius = 8f;

    private Transform target;
    private bool impacted = false;

    private void Start()
    {
        GameObject spaceship = GameObject.FindGameObjectWithTag("Spaceship");
        if (spaceship != null) target = spaceship.transform;
    }

    private void Update()
    {
        if (impacted || target == null) return;

        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        transform.LookAt(target);

        if (Vector3.Distance(transform.position, target.position) <= impactDistance)
        {
            Impact();
        }
    }

    private void Impact()
    {
        impacted = true;

        FixManager.Instance.broken(FixType.Engine);
        FixManager.Instance.broken(FixType.Wall);
        CreateResource.Instance.SpawnDebris(transform.position, debrisCount, debrisSpreadRadius);

        Destroy(gameObject);
    }
}