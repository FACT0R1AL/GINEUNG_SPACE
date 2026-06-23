using UnityEngine;
using UnityEngine.UI;

public class BoosterSystem : MonoBehaviour
{
    public float boostDuration = 7f;

    public float maxBoost = 7f;
    public float currentBoost = 0;

    public Image boostBarFill;

    public bool useBooster = false;

    public float boosterForce = 0.7f;

    public GameObject boosterViewCamera;
    public GameObject[] UIs;

    void Start()
    {
        currentBoost = maxBoost;
            if (boostBarFill != null)
            {
                boostBarFill.fillAmount = currentBoost / maxBoost;
            }
    }

    void Update()
    {
        if (!useBooster)
        {
            currentBoost += Time.deltaTime * 0.3f;
            if(boostBarFill != null)
            {
                boostBarFill.fillAmount = currentBoost / maxBoost;
            }
            if (currentBoost > maxBoost)
                currentBoost = maxBoost;
        }

        if (!GameManager.instance.inSpaceShipUI.activeSelf)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentBoost > 0)
            {
                useBooster = !useBooster;
                boosterViewCamera.SetActive(useBooster);
                foreach (var ui in UIs)
                {
                    ui.SetActive(!useBooster);
                }
            }
        }

        if (currentBoost <= 0)
        {
            useBooster = false;
            boosterViewCamera.SetActive(false);
            foreach (var ui in UIs)
            {
                if (ui.name == "Canvas")
                {
                    continue;
                }
                ui.SetActive(true);
            }
        }

        
        if (useBooster && currentBoost > 0)
        {
            currentBoost -= Time.deltaTime;
            if (currentBoost < 0)
                currentBoost = 0;
            if(boostBarFill != null)
            {
                boostBarFill.fillAmount = currentBoost / maxBoost;
            }
        }
    }
}
