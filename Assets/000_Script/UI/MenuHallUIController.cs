using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] TextMeshProUGUI goldText;
    private void Start()
    {
        weaponShop.onClick.AddListener(SwitchToShopUI);
        turnOffShop.onClick.AddListener(SwitchToMainHall);
        skinShop.onClick.AddListener(SwitchToSkinUI);
        turnOffSkinShopBtn.onClick.AddListener(SwitchToMainHall);
        AdsButton.onClick.AddListener(SwitchToRealShop);
        SwitchToMainHall();
        DataPersistenceManager.Instance.OnGoldChange.AddListener(UpdateGoldChange);
        UpdateGoldChange();
    }
    private void OnEnable()
    {
        
    }
    private void SwitchToMainHall()
    {
        GameManager.Instance.SetGameStates(Enum.GameState.Hall, Enum.GameplayState.PVE);
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
        GameManager.Instance.SetGameStates(Enum.GameState.SkinShop, Enum.GameplayState.PVE);
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

    private void UpdateGoldChange()
    {
        goldText.text = DataPersistenceManager.Instance.GameData.gold.ToString();
    }
}
