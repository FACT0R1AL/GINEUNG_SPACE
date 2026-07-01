using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (currentEvent == EventType.none)
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
        yield return new WaitForSeconds(15f);
        GameManager.instance.flashlightEventActive = false;
        GameManager.instance.spaceShip.maxMoveSpeed = defaultSpeed;
        currentEvent = EventType.none;
    }

    struct Message
        {
            public string text;
            public Text textComponent;
        }

    IEnumerator ElctriErrorEvent()
    {
        GameManager.instance.SendMessage("전파 통신 오류 이벤트 발생", Color.yellow);
        List<Message> messages = new List<Message>();
        foreach (var Text in FindObjectsByType<Text>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            messages.Add(new Message { text = Text.text, textComponent = Text });
            Text.text = "error";
        };
        GameManager.instance.elctriErrorEventActive = true;
        yield return new WaitForSeconds(8f);
        GameManager.instance.elctriErrorEventActive = false;
        for(int i = 0; i < messages.Count; i++)
        {
            if (messages[i].textComponent != null)
            {
                messages[i].textComponent.text = messages[i].text;
            }
        }
        currentEvent = EventType.none;
    }

    private void stoneHitEvent()
    {
        GameManager.instance.SendMessage("소행성 충돌 이벤트 발생", Color.yellow);
        FixManager.Instance.broken(FixType.Engine);
        FixManager.Instance.broken(FixType.Wall);
        currentEvent = EventType.none;
    }

    

}
