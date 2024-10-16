using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] Canvas UIGameplay;
    [SerializeField] Canvas UIHall;
    [SerializeField] Button PlayBTN;
    [SerializeField] Button MainMenuBTN;
    [SerializeField] Canvas thisCanvas;
    private void Awake()
    {
        PlayBTN.onClick.AddListener(SwitchToGamePlayUI);
        MainMenuBTN.onClick.AddListener(SwitchToHallUI);
        SwitchToHallUI();
    }
    private void SwitchToGamePlayUI()
    {
        TurnOffUI();
        UIGameplay.gameObject.SetActive(true);
        GameManager.Instance.SetGameState(Enum.GameState.Zone1);
        thisCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }
    private void SwitchToHallUI()
    {
        TurnOffUI();
        UIHall.gameObject.SetActive(true);
        GameManager.Instance.SetGameState(Enum.GameState.Hall);
        thisCanvas.renderMode = RenderMode.ScreenSpaceCamera;
    }
    private void TurnOffUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

}