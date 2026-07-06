using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    public SettingUI settingUI;
    public RankingUI rankingUI;
    
    public void GameStart()
    {
        Loading.LoadingScene("GameScene");
    }

    public void OpenSettings()
    {
        settingUI.gameObject.SetActive(true);
    }
    
    public void OpenRanking()
    {
        rankingUI.RankShow();
    }

    public void EexitGame()
    {
        Application.Quit();
    }
}
