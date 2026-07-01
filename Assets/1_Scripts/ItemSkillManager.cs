using UnityEngine;
using System.Collections;

public class ItemSkillManager : MonoBehaviour
{
    public Player player;
    public SpaceShip spaceShip;
    public float spaceShipSpeedUpValue = 50f;

    private Coroutine spaceSpeed;

    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            WarpItem();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpaceSpeedUp();
        }
    }

    public void WarpItem()
    {
        if (GameManager.instance.elctriErrorEventActive)
        {
            GameManager.instance.SendMessage("전파 통신 오류로 인한 아이템 사용불가", Color.yellow);
            return;
        }
        player.transform.position = spaceShip.transform.position + Random.onUnitSphere * 5f;
        player.transform.parent = spaceShip.transform;
        player.isInSpaceship = true;
        player.rb.isKinematic = true;
        player.currentVelocity = Vector3.zero;
        GameManager.instance.inSpaceShipUI.SetActive(true);
    }

    public void SpaceSpeedUp()
    {
        if (GameManager.instance.elctriErrorEventActive)
        {
            GameManager.instance.SendMessage("전파 통신 오류로 인한 아이템 사용불가", Color.yellow);
            return;
        }
        if (spaceSpeed != null)
        {
            StopCoroutine(spaceSpeed);
            spaceShip.maxMoveSpeed -= spaceShipSpeedUpValue; // 이전 효과 제거
            spaceSpeed = null;
        }
        spaceSpeed = StartCoroutine(SpaceSpeedUpCoroutine(20f, spaceShipSpeedUpValue));
    }

    private IEnumerator SpaceSpeedUpCoroutine(float duration, float value)
    {
        float originalSpeed = spaceShip.maxMoveSpeed;
        spaceShip.maxMoveSpeed += value;

        yield return new WaitForSeconds(duration);

        spaceShip.maxMoveSpeed = originalSpeed;
        spaceSpeed = null;
    }
}
