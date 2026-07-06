using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CreateResource : MonoBehaviour
{
    public static CreateResource Instance { get; private set; }

    [Header("Pool Settings")]
    public ResourcePrefabEntry[] resourcePrefabEntries;
    public int poolSizePerType = 5;

    [Header("Spawn Settings")]
    public int maxActiveCount = 20;
    public float minSpawnDistance = 30f;
    public float maxSpawnDistance = 150f;

    [Tooltip("우주선 뒤쪽으로 이 거리 이상 떨어지면 회수")]
    public float behindDespawnDistance = 80f;
    [Tooltip("옆/앞 방향으로 이 거리 이상이면 회수")]
    public float sideDespawnDistance = 200f;

    [Header("Solar Storm Event Settings")]
    public int stormResourceCount = 8;
    public float stormResourceLifetime = 20f;

    private Dictionary<int, Queue<GameObject>> pools = new Dictionary<int, Queue<GameObject>>();
    private List<GameObject> activeResources = new List<GameObject>();
    private List<GameObject> stormResources = new List<GameObject>();

    private GameObject spaceship;
    private SpaceShip spaceshipComp;
    private Transform poolRoot;

    private Vector3 prevSpaceshipPos;
    private Vector3 spaceshipMoveDir = Vector3.forward;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }

    private void Start()
    {
        poolRoot = new GameObject("ResourcePoolRoot").transform;

        spaceship = GameObject.FindGameObjectWithTag("Spaceship");
        if (spaceship != null)
        {
            spaceshipComp = spaceship.GetComponent<SpaceShip>();
            prevSpaceshipPos = spaceship.transform.position;
        }

        InitPools();
        FillToMax();
    }

    private void Update()
    {
        UpdateMoveDir();
        DespawnFarResources();
        FillToMax();
    }

    private void UpdateMoveDir()
    {
        if (spaceship == null) return;
        Vector3 delta = spaceship.transform.position - prevSpaceshipPos;
        if (delta.sqrMagnitude > 0.0001f)
            spaceshipMoveDir = delta.normalized;
        prevSpaceshipPos = spaceship.transform.position;
    }

    private void InitPools()
    {
        for (int i = 0; i < resourcePrefabEntries.Length; i++)
        {
            int key = i;
            pools[key] = new Queue<GameObject>();
            for (int j = 0; j < poolSizePerType; j++)
            {
                GameObject obj = Instantiate(resourcePrefabEntries[key].prefab, poolRoot);
                obj.SetActive(false);
                pools[key].Enqueue(obj);
            }
        }
    }

    private void FillToMax()
    {
        while (activeResources.Count < maxActiveCount)
        {
            if (!SpawnOne()) break;
        }
    }

    private bool SpawnOne()
    {
        if (resourcePrefabEntries.Length == 0) return false;

        int typeIndex = Random.Range(0, resourcePrefabEntries.Length);

        if (!pools.ContainsKey(typeIndex) || pools[typeIndex].Count == 0)
        {
            GameObject extra = Instantiate(resourcePrefabEntries[typeIndex].prefab, poolRoot);
            extra.SetActive(false);
            pools[typeIndex].Enqueue(extra);
        }

        Vector3 spawnPos = GetRandomSpawnPosition();
        if (spawnPos == Vector3.zero) return false;

        GameObject obj = pools[typeIndex].Dequeue();
        obj.transform.position = spawnPos;
        obj.transform.rotation = Random.rotation;
        obj.SetActive(true);

        var res = obj.GetComponent<Resource>();
        if (res != null)
        {
            var entry = resourcePrefabEntries[typeIndex];
            res.itemType = entry.itemType;
            res.count = Random.Range(entry.minCount, entry.maxCount + 1);
            res.poolTypeIndex = typeIndex;
            res.ResourceUI.SetActive(false);
        }

        activeResources.Add(obj);
        return true;
    }

    private bool SpawnOne(ItemType itemType, out GameObject spawnedObj)
    {
        spawnedObj = null;
        Vector3 spawnPos = GetRandomSpawnPosition();
        if (spawnPos == Vector3.zero) return false;

        return SpawnOneAt(itemType, spawnPos, out spawnedObj);
    }

    private bool SpawnOneAt(ItemType itemType, Vector3 position, out GameObject spawnedObj)
    {
        spawnedObj = null;
        if (resourcePrefabEntries.Length == 0) return false;

        int typeIndex = resourcePrefabEntries.ToList().FindIndex(entry => entry.itemType == itemType);
        if (typeIndex < 0) return false;

        if (!pools.ContainsKey(typeIndex) || pools[typeIndex].Count == 0)
        {
            GameObject extra = Instantiate(resourcePrefabEntries[typeIndex].prefab, poolRoot);
            extra.SetActive(false);
            pools[typeIndex].Enqueue(extra);
        }

        GameObject obj = pools[typeIndex].Dequeue();
        obj.transform.position = position;
        obj.transform.rotation = Random.rotation;
        obj.SetActive(true);

        var res = obj.GetComponent<Resource>();
        if (res != null)
        {
            var entry = resourcePrefabEntries[typeIndex];
            res.itemType = entry.itemType;
            res.count = Random.Range(entry.minCount, entry.maxCount + 1);
            res.poolTypeIndex = typeIndex;
            res.ResourceUI.SetActive(false);
        }

        activeResources.Add(obj);
        spawnedObj = obj;
        return true;
    }

    private static readonly ItemType[] StormLv1Types = { ItemType.IronLv1, ItemType.CopperLv1, ItemType.PlasticLv1 };
    private static readonly ItemType[] DebrisTypes =
    {
        ItemType.IronLv1, ItemType.IronLv2,
        ItemType.CopperLv1, ItemType.CopperLv2,
        ItemType.PlasticLv1, ItemType.PlasticLv2
    };

    public void SpawnStormResources()
    {
        for (int i = 0; i < stormResourceCount; i++)
        {
            ItemType type = StormLv1Types[Random.Range(0, StormLv1Types.Length)];
            if (SpawnOne(type, out GameObject obj))
            {
                stormResources.Add(obj);
            }
        }

        StartCoroutine(ClearStormResourcesAfter(stormResourceLifetime));
    }

    public void SpawnDebris(Vector3 center, int count, float spreadRadius)
    {
        for (int i = 0; i < count; i++)
        {
            ItemType type = DebrisTypes[Random.Range(0, DebrisTypes.Length)];
            Vector3 pos = center + Random.insideUnitSphere * spreadRadius;
            SpawnOneAt(type, pos, out _);
        }
    }

    private IEnumerator ClearStormResourcesAfter(float delay)
    {
        yield return new WaitForSeconds(delay);

        foreach (var obj in stormResources)
        {
            if (obj != null && obj.activeSelf)
            {
                ReturnToPool(obj);
            }
        }
        stormResources.Clear();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        if (spaceship == null) return Vector3.zero;

        Vector3 center = spaceship.transform.position;

        for (int attempt = 0; attempt < 15; attempt++)
        {
            Vector3 dir = Random.onUnitSphere;
            float dist = Random.Range(minSpawnDistance, maxSpawnDistance);
            Vector3 pos = center + dir * dist;

            if (!Physics.CheckSphere(pos, 3f))
                return pos;
        }
        return Vector3.zero;
    }

    private void DespawnFarResources()
    {
        if (spaceship == null) return;

        Vector3 center = spaceship.transform.position;

        for (int i = activeResources.Count - 1; i >= 0; i--)
        {
            GameObject obj = activeResources[i];
            if (obj == null)
            {
                activeResources.RemoveAt(i);
                continue;
            }

            Vector3 toObj = obj.transform.position - center;
            float forwardDot = Vector3.Dot(toObj, spaceshipMoveDir);
            float lateralDist = Vector3.Distance(obj.transform.position, center + spaceship.transform.forward * forwardDot);

            // 뒤로 너무 멀어지거나 옆/앞으로 너무 멀어지면 회수
            bool tooFarBehind = forwardDot < -behindDespawnDistance;
            bool tooFarAway = lateralDist > sideDespawnDistance || toObj.magnitude > sideDespawnDistance;

            if (tooFarBehind || tooFarAway)
            {
                TeleportToFront(obj);
            }
        }
    }

    private void TeleportToFront(GameObject obj)
    {
        Vector3 newPos = GetRandomSpawnPosition();
        if (newPos == Vector3.zero) return;
        obj.transform.position = newPos;
        obj.transform.rotation = Random.rotation;

        var res = obj.GetComponent<Resource>();
        if (res != null)
            res.ResourceUI.SetActive(false);
    }

    public float respawnDelay = 5f;

    public void ReturnToPoolDelayed(GameObject obj)
    {
        activeResources.Remove(obj);
        obj.SetActive(false);
        StartCoroutine(DelayedReturn(obj));
    }

    private IEnumerator DelayedReturn(GameObject obj)
    {
        yield return new WaitForSeconds(respawnDelay);

        Vector3 spawnPos = GetRandomSpawnPosition();
        if (spawnPos != Vector3.zero)
        {
            obj.transform.position = spawnPos;
            obj.transform.rotation = Random.rotation;
            obj.SetActive(true);

            var res = obj.GetComponent<Resource>();
            if (res != null)
                res.ResourceUI.SetActive(false);

            activeResources.Add(obj);
        }
        else
        {
            var res = obj.GetComponent<Resource>();
            if (res != null && pools.ContainsKey(res.poolTypeIndex))
                pools[res.poolTypeIndex].Enqueue(obj);
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        var res = obj.GetComponent<Resource>();
        if (res != null && pools.ContainsKey(res.poolTypeIndex))
        {
            obj.SetActive(false);
            pools[res.poolTypeIndex].Enqueue(obj);
        }
        else
        {
            obj.SetActive(false);
        }
        activeResources.Remove(obj);
    }
}

[System.Serializable]
public struct ResourcePrefabEntry
{
    public GameObject prefab;
    public ItemType itemType;
    public int minCount;
    public int maxCount;
}
