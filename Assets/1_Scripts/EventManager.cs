using UnityEngine;
using System.Collections;

public enum EventType
{
    none,
    flashlight,
    elctriError,
    stoneHit,
    gaspung
}


[System.Serializable]
public struct EventData
{
    public EventType eventType;
    public float percentage;
}

public class EventManager : MonoBehaviour
{
    public EventType currentEvent = EventType.none;
    public EventData[] eventData = new EventData[3];
    public float cooldownDuration = 20f;

    [Header("Asteroid Event Settings")]
    public GameObject asteroidPrefab;
    public float asteroidSpawnDistance = 100f;

    private bool onCooldown = false;

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (currentEvent == EventType.none && !onCooldown)
            {
                CheckTime();
            }
        }
    }

    private void CheckTime()
    {
        for (int i = 0; i < eventData.Length; i++)
        {
            float randomValue = Random.Range(0f, 100f);

            if (randomValue <= eventData[i].percentage)
            {
                switch (eventData[i].eventType)
                {
                    case EventType.flashlight:
                        currentEvent = EventType.flashlight;
                        
                        StartCoroutine(FlashlightEvent());
                        break;
                    case EventType.elctriError:
                        currentEvent = EventType.elctriError;
                        
                        StartCoroutine(ElctriErrorEvent());
                        break;
                    case EventType.stoneHit:
                        currentEvent = EventType.stoneHit;
                        stoneHitEvent();
                        break;
                }
            }
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StartCoroutine(FlashlightEvent());
        }
        if(Input.GetKeyDown(KeyCode.Alpha9))
        {
            StartCoroutine(ElctriErrorEvent());
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            stoneHitEvent();
        }
    }

    IEnumerator FlashlightEvent()
    {
        GameManager.instance.SendMessage("태양 폭풍 이벤트 발생", Color.yellow);
        float defaultSpeed = GameManager.instance.spaceShip.maxMoveSpeed;
        GameManager.instance.flashlightEventActive = true;
        GameManager.instance.spaceShip.maxMoveSpeed /= 2f;
        CreateResource.Instance.SpawnStormResources();
        yield return new WaitForSeconds(15f);
        GameManager.instance.flashlightEventActive = false;
        GameManager.instance.spaceShip.maxMoveSpeed = defaultSpeed;
        EndEvent();
    }

    IEnumerator ElctriErrorEvent()
    {
        GameManager.instance.SendMessage("전파 통신 오류 이벤트 발생", Color.yellow);
        GameManager.instance.elctriErrorEventActive = true;
        yield return new WaitForSeconds(8f);
        GameManager.instance.elctriErrorEventActive = false;
        EndEvent();
    }

    private void stoneHitEvent()
    {
        GameManager.instance.SendMessage("소행성 충돌 이벤트 발생", Color.yellow);

        if (asteroidPrefab != null && GameManager.instance.spaceShip != null)
        {
            Vector3 spawnPos = GameManager.instance.spaceShip.transform.position
                + Random.onUnitSphere * asteroidSpawnDistance;
            Instantiate(asteroidPrefab, spawnPos, Quaternion.identity);
        }

        EndEvent();
    }

    private void EndEvent()
    {
        currentEvent = EventType.none;
        StartCoroutine(CooldownRoutine());
    }

    private IEnumerator CooldownRoutine()
    {
        onCooldown = true;
        yield return new WaitForSeconds(cooldownDuration);
        onCooldown = false;
    }

}
