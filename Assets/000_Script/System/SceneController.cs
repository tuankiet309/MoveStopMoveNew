using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;
    public static SceneController Instance { get { return instance; } }

    private AsyncOperation asyncOperation;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == Enum.SceneName.LoadingScene.ToString())
        {
            StartCoroutine(LoadLoadingSceneAndAsyncFirstTime());
        }
    }
    private IEnumerator LoadLoadingSceneAndAsyncFirstTime()
    {
        asyncOperation = SceneManager.LoadSceneAsync(Enum.SceneName.PVEScene.ToString());
        asyncOperation.allowSceneActivation = false; 

      

        while (asyncOperation.progress < 0.9f)
        {
            yield return null; 
        }

        asyncOperation.allowSceneActivation = true;

    }

    public void LoadSceneRightAway(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAsyncWay(string sceneName)
    {
        StartCoroutine(LoadLoadingSceneAndAsync(sceneName));
    }

    private IEnumerator LoadLoadingSceneAndAsync(string sceneName)
    {
        AsyncOperation loadingSceneLoad = SceneManager.LoadSceneAsync(Enum.SceneName.LoadingScene.ToString());
        while (!loadingSceneLoad.isDone)
        {
            yield return null;
        }

        LoadingScreenUI loadingScreenUI = FindObjectOfType<LoadingScreenUI>();
        if (loadingScreenUI != null)
        {
            loadingScreenUI.ShowRandomPanel();
        }

        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }


        asyncOperation.allowSceneActivation = true;

    }
}