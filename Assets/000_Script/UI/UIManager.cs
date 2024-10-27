using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] Canvas UIGameplay;
    [SerializeField] Canvas UIHall;
    [SerializeField] Button PlayBTN;
    [SerializeField] Button MainMenuBTN;
    [SerializeField] Canvas thisCanvas;
    [SerializeField] TMP_InputField inputName;

   

    private void Awake()
    {
        PlayBTN.onClick.AddListener(SwitchToGamePlayUI);
        MainMenuBTN.onClick.AddListener(SwitchToHallUI);
        
    }
    private void Start()
    {
        inputName.onEndEdit.AddListener(UpdateNameForPlayer);
        inputName.text = Player.Instance.GetComponent<ActorInformationController>().Name;
        SwitchToHallUI();
    }
    private void SwitchToGamePlayUI()
    {
        UIGameplay.gameObject.SetActive(true);
        GameManager.Instance.SetGameStates(Enum.GameState.Ingame, Enum.InGameState.PVE);
        thisCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
    }
    private void SwitchToHallUI()
    {
        TurnOffUI();
        UIHall.gameObject.SetActive(true);
        GameManager.Instance.SetGameStates(Enum.GameState.Hall, Enum.InGameState.PVE);
        thisCanvas.renderMode = RenderMode.ScreenSpaceCamera;
    }
    private void TurnOffUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    private void UpdateNameForPlayer(string newName)
    {
        Player.Instance.GetComponent<ActorInformationController>().UpdateName(newName);
    }

}
