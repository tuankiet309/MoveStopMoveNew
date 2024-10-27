using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinUIController : MonoBehaviour
{
    [SerializeField] Button PlayZone2;
    [SerializeField] TextMeshProUGUI goldText;

    private void Start()
    {
        PlayZone2.onClick.AddListener(GoToNextZone);
        SceneController.Instance.LoadSceneAsyncWay(SceneManager.GetActiveScene());
        goldText.text = PlayerGoldInGameController.Instance.Gold.ToString();

    }

    private void GoToNextZone()
    {
        PlayerGoldInGameController.Instance.OnEndCurrentLevel();
        LevelManager.Instance.CurrentPVELevel++;
        SceneController.Instance.AddThisEventToActiveScene();
    }
}
