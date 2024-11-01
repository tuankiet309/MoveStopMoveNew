using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButtonUI : MonoBehaviour
{
    [SerializeField] private Button Button;
    [SerializeField] private Enum.SceneName sceneName;

    private void Start()
    {
        Button.onClick.RemoveAllListeners();
        Button.onClick.AddListener(EnterScene);
    }
    private void EnterScene()
    {
        SceneController.Instance.LoadSceneAsyncWay(sceneName.ToString());
    }
}
