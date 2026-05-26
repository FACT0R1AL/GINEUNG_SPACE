using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public Text spaceshipVelocityText;
	public Image spaceshipVelocityImage;

	public Text oxygenTankText;
	public Image oxygenTankImage;

	public Text remainingDistanceText;

	public Text timerText;

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
		passedPathLength += spaceship.GetComponent<SpaceShip>().currentMoveSpeed * Time.deltaTime * 1000f + 1f;

		float path = spaceshipPathLength - passedPathLength;
		path = Mathf.Clamp(path, 0, spaceshipPathLength);
		float path_man = path / 10000f;
		float path_cheon = path % 10000f;

		remainingDistanceText.text = $"{path.ToString("#,##0")}km 남음";

		spaceshipVelocityText.text = $"{Mathf.Round(spaceship.GetComponent<SpaceShip>().currentMoveSpeed * 1000f)}km/s";
		spaceshipVelocityImage.fillAmount = (spaceship.GetComponent<SpaceShip>().currentMoveSpeed * 1000f) / (spaceship.GetComponent<SpaceShip>().maxMoveSpeed * 1000f);

		oxygenTankImage.fillAmount = player.GetComponent<Player>().currentOxygen / player.GetComponent<Player>().maxOxygen;
		oxygenTankText.text = $"{Mathf.Round((player.GetComponent<Player>().currentOxygen / player.GetComponent<Player>().maxOxygen) * 100f)}%";
		oxygenTankText.GetComponent<RectTransform>().localPosition =
			new Vector3(oxygenTankText.GetComponent<RectTransform>().localPosition.x,
						170f * oxygenTankImage.fillAmount - 60f,
						oxygenTankText.GetComponent<RectTransform>().localPosition.z);

		// ----------------------------------------------------
		// [수정된 타이머 계산부] 
		// 기존 스타일을 유지하면서 시간 계산 오류만 수정했습니다.
		// ----------------------------------------------------

		// 1. maxMoveSpeed 대신 현재 속도(currentMoveSpeed)로 나누어 실시간 남은 '초' 계산
		float remainingTime = 0f;
		float currentSpeed = spaceship.GetComponent<SpaceShip>().currentMoveSpeed * 1000f;

		if (currentSpeed > 0)
		{
			remainingTime = path / currentSpeed;
		}

		// 예외 처리 (시간이 음수가 되거나 먹통이 되는 현상 방지)
		if (float.IsNaN(remainingTime) || remainingTime < 0) remainingTime = 0f;

		// 2. TimeSpan 구조체를 이용해 시:분:초 형식으로 올바르게 변환
		System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(remainingTime);

		// 3. UI 텍스트에 반영 (60초가 넘으면 분으로, 60분이 넘으면 시간으로 정상 출력)
		timerText.text = string.Format("{0:00}:{1:00}.{2:00}", (int)timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
	}
}