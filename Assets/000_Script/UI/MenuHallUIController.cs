using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHallUIController : MonoBehaviour
{
    [SerializeField] Canvas menuCanvas;
    [SerializeField] Canvas shopWeaponCanvas;
    [SerializeField] Button weaponShop;
    [SerializeField] Button turnOffShop;

    private void Start()
    {
        weaponShop.onClick.AddListener(SwitchToShopUI);
        turnOffShop.onClick.AddListener(SwitchToMainHall);
        SwitchToMainHall();
    }
    private void SwitchToMainHall()
    {
        TurnOffUI();
        menuCanvas.gameObject.SetActive(true);
    }    
    private void SwitchToShopUI()
    {
        TurnOffUI();
        shopWeaponCanvas.gameObject.SetActive(true);
    }
    private void TurnOffUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
