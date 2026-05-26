using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageSlot : MonoBehaviour
{
    public Text messageText;
    public Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }

    public void SetMessage(string message)
    {
        messageText.text = message;
        StartCoroutine(DestroyAfterTime(2.7f));
    }

    public void SetMessage(string message, Color color)
    {
        messageText.text = message;
        messageText.color = color;
        StartCoroutine(DestroyAfterTime(2.7f));
    }

    IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        animator.SetTrigger("Hide");
    }
}
