using UnityEngine;

public class GasZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Spaceship") && !other.CompareTag("SpaceshipHitbox")) return;

        GameManager.instance.gasExplosionEventActive = true;
        GameManager.instance.SendMessage("가스 폭발 지역 진입: 산소 탱크 손상", Color.yellow);
        FixManager.Instance.broken(FixType.Oxygen);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Spaceship") && !other.CompareTag("SpaceshipHitbox")) return;

        GameManager.instance.gasExplosionEventActive = false;
    }
}
