using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHallUIController : MonoBehaviour
{
    [SerializeField] Canvas menuCanvas;
    [SerializeField] Canvas shopWeaponCanvas;
    [SerializeField] Canvas shopSkinCanvas;
    [SerializeField] Canvas realMoneyCanvas;
    [SerializeField] Button AdsButton;
    [SerializeField] Button weaponShop;
    [SerializeField] Button turnOffShop;
    [SerializeField] Button turnOffSkinShopBtn;
    [SerializeField] Button skinShop;

    private void Start()
    {
        weaponShop.onClick.AddListener(SwitchToShopUI);
        turnOffShop.onClick.AddListener(SwitchToMainHall);
        skinShop.onClick.AddListener(SwitchToSkinUI);
        turnOffSkinShopBtn.onClick.AddListener(SwitchToMainHall);
        AdsButton.onClick.AddListener(SwitchToRealShop);
        SwitchToMainHall();
    }
    private void SwitchToMainHall()
    {
        GameManager.Instance.SetGameState(Enum.GameState.Hall);
        TurnOffUI();
        menuCanvas.gameObject.SetActive(true);
        
    }    
    private void SwitchToShopUI()
    {
        
        shopWeaponCanvas.gameObject.SetActive(true);
        
    }
    private void SwitchToSkinUI()
    {
        
        shopSkinCanvas.gameObject.SetActive(true);
        GameManager.Instance.SetGameState(Enum.GameState.SkinShop);
    }
    private void SwitchToRealShop()
    {
        realMoneyCanvas.gameObject.SetActive(true);

    }
    private void TurnOffUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void Clic()
    {
        Debug.Log("I am clicked");
    }
}
