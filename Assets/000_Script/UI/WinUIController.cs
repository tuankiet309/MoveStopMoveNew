using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinUIController : MonoBehaviour
{
    [SerializeField] Button PlayZone2;

    private void Start()
    {
        PlayZone2.onClick.AddListener(GoToZone2);
        SceneController.Instance.LoadSceneAsyncWay(SceneManager.GetActiveScene());
    }
   
    private void GoToZone2()
    {
        SceneController.Instance.AddThisEventToActiveScene();
    }
}
