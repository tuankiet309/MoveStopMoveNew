using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ZombieCityButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] Button button;

    private void Start()
    {
        //button.onClick.AddListener(() => SceneController.Instance.LoadSceneAsyncWay(Enum.SceneName.ZCScene.ToString()));
        //levelText.text = (LevelManager.Instance.CurrentZCLevel + 1).ToString();
    }

}
