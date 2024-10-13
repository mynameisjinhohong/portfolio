using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public TMP_Text loadingText;
    public float minWait = 0.5f;
    static string nextScene = "StartScene";
    public Slider loadingSlider;
    // Start is called before the first frame update
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LoadingScene");

    }
    private void Start()
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneCo());

    }

    // Update is called once per frame
    IEnumerator LoadSceneCo()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(nextScene);
        float currentTime = 0;
        float loadingTime = 0;
        async.allowSceneActivation = false;
        while (!async.isDone)
        {
            currentTime += Time.deltaTime;
            loadingTime+= Time.deltaTime;
            yield return null;
            loadingSlider.value = Mathf.Lerp(loadingSlider.value,async.progress,loadingTime);
            loadingText.text = Mathf.Floor(loadingSlider.value*1000)/10f + "%";
            if (loadingSlider.value >= async.progress)
            {
                loadingTime = 0f;
            }
            if (currentTime > minWait)
            {
                async.allowSceneActivation = true;
                break;
            }

        }
    }
}
