using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;
    public static SceneController Instance { get { return instance; } }

    private AsyncOperation asyncOperation;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    public void LoadSceneRightAway(Scene scene)
    {
        Debug.Log(scene.buildIndex);
        DataPersistenceManager.Instance.SaveGame();
        SceneManager.LoadScene(-scene.buildIndex);
    }
    public void LoadSceneAsyncWay(Scene scene)
    {
        StartCoroutine(LoadSceneAsync(scene));
    }

    public void AddThisEventToActiveScene()
    {
        DataPersistenceManager.Instance.SaveGame();
        asyncOperation.allowSceneActivation = true;
    }
    private IEnumerator LoadSceneAsync( Scene scene)
    {
        asyncOperation = SceneManager.LoadSceneAsync(scene.buildIndex);
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }
        
    }
}
