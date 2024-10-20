using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinUIController : MonoBehaviour
{
    [SerializeField] Button PlayZone2;

    private AsyncOperation asyncOperation;
    private void Start()
    {
        PlayZone2.onClick.AddListener(GoToZone2);
        StartCoroutine(LoadSceneAsync());

    }
    private IEnumerator LoadSceneAsync()
    {
        asyncOperation = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        asyncOperation.allowSceneActivation = false;
        while (asyncOperation.progress < 0.9f)
        {
            yield return null;
        }
        PlayZone2.gameObject.SetActive(true);
    }
    private void GoToZone2()
    {
        asyncOperation.allowSceneActivation = true;
    }
}
