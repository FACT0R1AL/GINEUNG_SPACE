using UnityEngine;

public class MessageUI : MonoBehaviour
{
    public Transform messagePanel;
    public GameObject messagePrefab;

    public void SendMessage(string message)
    {
        GameObject newMessage = Instantiate(messagePrefab, messagePanel);
        newMessage.GetComponent<MessageSlot>().SetMessage(message);
        Destroy(newMessage, 3f); // 3초 후에 메시지 삭제
    }

    public void SendMessage(string message, Color color)
    {
        GameObject newMessage = Instantiate(messagePrefab, messagePanel);
        newMessage.GetComponent<MessageSlot>().SetMessage(message, color);
        Destroy(newMessage, 3f); // 3초 후에 메시지 삭제
    }
}
