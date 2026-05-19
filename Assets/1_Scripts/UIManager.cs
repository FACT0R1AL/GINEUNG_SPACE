using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text spaceshipVelocityText;
    public Image spaceshipVelocityImage;

    public Text oxygenTankText;
    public Image oxygenTankImage;

    public Text remainingDistanceText;

    private GameObject player;
    private GameObject spaceship;

	private float spaceshipPathLength;
	private float passedPathLength;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		spaceship = GameObject.FindGameObjectWithTag("Spaceship");

		spaceshipPathLength = spaceship.GetComponent<SpaceShip>().pathObj.GetComponent<PathMaker>().pathLength * 1000f;
	}

	private void Update()
	{
		passedPathLength += spaceship.GetComponent<SpaceShip>().currentMoveSpeed * Time.deltaTime * 1000f;

		float path = spaceshipPathLength - passedPathLength;
		path = Mathf.Clamp(path, 0, spaceshipPathLength);

		remainingDistanceText.text = $"{Mathf.Round(path / 1000f).ToString("#,##0")}¸¸km ³²À½";

		spaceshipVelocityText.text = $"{Mathf.Round(spaceship.GetComponent<SpaceShip>().currentMoveSpeed * 1000f)}km/s";
		spaceshipVelocityImage.fillAmount = (spaceship.GetComponent<SpaceShip>().currentMoveSpeed * 1000f) / (spaceship.GetComponent<SpaceShip>().maxMoveSpeed * 1000f);

		oxygenTankImage.fillAmount = player.GetComponent<Player>().currentOxygen / player.GetComponent<Player>().maxOxygen;
		oxygenTankText.text = $"{Mathf.Round((player.GetComponent<Player>().currentOxygen / player.GetComponent<Player>().maxOxygen) * 100f)}%";
		oxygenTankText.GetComponent<RectTransform>().localPosition =
			new Vector3(oxygenTankText.GetComponent<RectTransform>().localPosition.x,
						170f * oxygenTankImage.fillAmount - 60f,
						oxygenTankText.GetComponent<RectTransform>().localPosition.z);
	}
}
