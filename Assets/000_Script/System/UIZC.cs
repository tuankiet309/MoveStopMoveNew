using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIZC : MonoBehaviour
{
    [SerializeField] private Canvas Upgrade;
    [SerializeField] private Canvas PowerUpCanvas;
    [SerializeField] private Canvas IngameCanvas;
    [SerializeField] private Canvas OnEndGame;
    [SerializeField] private Canvas Setting;
        
    [SerializeField] private RectTransform gold;
    [SerializeField] private Button setting;
    [SerializeField] private Button restart;
    [SerializeField] private Button continues;

    private void Awake()
    {
        setting.onClick.AddListener(OpenSetting);
        restart.onClick.AddListener(Restart);
        continues.onClick.AddListener(ContinueGame);
    }
    private void Start()
    {
        GameManager.Instance.onStateChange.AddListener(UpdateCanvas);
        UpdateCanvas(GameManager.Instance.CurrentGameState,GameManager.Instance.CurrentInGameState);
        
    }

    private void UpdateCanvas(Enum.GameState state, Enum.GameplayState inGameState)
    {
        if(state == Enum.GameState.Dead || state == Enum.GameState.Win || state == Enum.GameState.Revive)
        {
            OnEndGameHappen();
        }
        if(state == Enum.GameState.Begin)
        {
            OnGameBegin();
        }
        if(state == Enum.GameState.Ingame)
        {
            OnInInGame();
        }

    }
    private void OnInInGame()
    {
        Upgrade.gameObject.SetActive(true);
        gold.gameObject.SetActive(true);
        setting.gameObject.SetActive(false);
        PowerUpCanvas.gameObject.SetActive(true);
    }
    private void OnGameBegin()
    {
        Upgrade.gameObject.SetActive(false);
        gold.gameObject.SetActive(false);
        setting.gameObject.SetActive(true);
        PowerUpCanvas.gameObject.SetActive(false);
    }
    private void OnEndGameHappen()
    {
        OnEndGame.gameObject.SetActive(true);  
        IngameCanvas.gameObject.SetActive(false);
    }

    private void OpenSetting()
    {
        Setting.gameObject.SetActive(true);  
        Time.timeScale = 0;  
    }
    private void ContinueGame()
    {
        Time.timeScale = 1;
        Setting.gameObject.SetActive(false);
    }
    private void Restart()
    {
        Time.timeScale = 1;
        SceneController.Instance.LoadSceneRightAway(Enum.SceneName.ZCScene.ToString());
    }

}
