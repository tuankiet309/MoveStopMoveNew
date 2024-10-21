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
        button.onClick.AddListener(() => GameManager.Instance.SetGameStates(Enum.GameState.Ingame, Enum.InGameState.Zombie));
        button.onClick.AddListener(() => SceneController.Instance.LoadSceneRightAway(SceneManager.GetSceneByBuildIndex(1)));
    }
    
}
