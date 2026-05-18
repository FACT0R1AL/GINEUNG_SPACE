using UnityEngine;
using UnityEngine.UI;

public class FixSlotButton : MonoBehaviour
{
    private Button fixButton;
    public FixUI fixUI;
    public FixType fixType;


    private void Awake()
    {
        fixButton = GetComponent<Button>();
        fixButton.onClick.AddListener(OnFixButtonClicked);
    }

    private void OnFixButtonClicked()
    {
        switch (fixType)
        {
            case FixType.Engine:
                if (FixManager.Instance.brokenEngine)
                {
                    fixUI.Show(fixType);
                }
                break;
            case FixType.Wall:
                if (FixManager.Instance.brokenWall)
                {
                    fixUI.Show(fixType);
                }
                break;
            case FixType.Oxygen:
                if (FixManager.Instance.brokenOxygen)
                {
                    fixUI.Show(fixType);
                }
                break;
            case FixType.Drone:
                if (FixManager.Instance.brokenDrone)
                {
                    fixUI.Show(fixType);
                }
                break;
        }
        
    }
}
