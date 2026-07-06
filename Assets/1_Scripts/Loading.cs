using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public Slider slider;
    public Text progressText;
    public float minLoadTime = 2f;
    public float gaugeFillSpeed = 0.6f;

    private static string targetSceneName;

    public static void LoadingScene(string sceneName)
    {
        targetSceneName = sceneName;
        SceneManager.LoadScene("LoadingScene", LoadSceneMode.Single);
    }

    private void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Single);
        op.allowSceneActivation = false;

        float elapsed = 0f;
        float displayedProgress = 0f;

        while (displayedProgress < 1f)
        {
            elapsed += Time.deltaTime;

            float realProgress = Mathf.Clamp01(op.progress / 0.9f);
            float timeProgress = Mathf.Clamp01(elapsed / minLoadTime);
            float targetProgress = Mathf.Min(realProgress, timeProgress);

            displayedProgress = Mathf.MoveTowards(displayedProgress, targetProgress, gaugeFillSpeed * Time.deltaTime);

            if (slider != null) slider.value = displayedProgress;
            if (progressText != null) progressText.text = $"{Mathf.RoundToInt(displayedProgress * 100)}%";

            yield return null;
        }

        op.allowSceneActivation = true;
    }
}