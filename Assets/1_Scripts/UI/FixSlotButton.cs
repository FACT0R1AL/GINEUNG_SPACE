using UnityEngine;
using UnityEngine.UI;

public class FixSlotButton : MonoBehaviour
{
    private Button fixButton;
    

    private void Awake()
    {
        fixButton = GetComponent<Button>();
        fixButton.onClick.AddListener(OnFixButtonClicked);
    }

    private void OnFixButtonClicked()
    {

    }
}
