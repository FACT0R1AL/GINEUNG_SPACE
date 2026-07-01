using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUI : MonoBehaviour
{
    public void GameStart()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void EexitGame()
    {
        Application.Quit();
    }
}
