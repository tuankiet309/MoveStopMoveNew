using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZCPowerUpUI : MonoBehaviour
{
    [SerializeField] private ZCPower[] ZCPowers;
    [SerializeField] Image imageDisplay;
    [SerializeField] TextMeshProUGUI nameOfPower;

    [SerializeField] Button NoThanks;
    [SerializeField] Button Pick;
    [SerializeField] Button GoBackToHome;
    [SerializeField] Button SwitchButton;
    [SerializeField] Canvas CountDown;
    [SerializeField] Canvas UpgradePanel;

    List<ZCPower> CurrentZCList = new List<ZCPower>();

    int CurrentZCIndex = 0;

    private void Start()
    {
        int random1 = -1;
        int random2 = -1;
        CurrentZCList.Clear();
        if (ZCPowers.Length >= 2)
        {
            random1 = Random.Range(0, ZCPowers.Length);
            do
            {
                random2 = Random.Range(0, ZCPowers.Length);
            }
            while (random1 == random2);
        }
        CurrentZCList.Add(ZCPowers[random1]);
        CurrentZCList.Add(ZCPowers[random2]);
        NoThanks.onClick.AddListener(CloseThisStuff);
        SetUpForThisZCPower(CurrentZCList[CurrentZCIndex]);
        SwitchButton.onClick.AddListener( () => SetUpForThisZCPower(CurrentZCList[CurrentZCIndex%CurrentZCList.Count ]) );


    }


    private void SetUpForThisZCPower(ZCPower zCPower)
    {
        imageDisplay.sprite = zCPower.PowerImage;
        nameOfPower.text = zCPower.PowerName;
        Pick.onClick.RemoveAllListeners();
        Pick.onClick.AddListener(() => { Player.Instance.GetComponent<ZCAttributeController>().SetZCPower(zCPower); });
        Pick.onClick.AddListener(CloseThisStuff );
        CurrentZCIndex++;        
    }

    private void CloseThisStuff()
    {
        gameObject.SetActive(false);
        UpgradePanel.gameObject.SetActive(false);
        CountDown.gameObject.SetActive(true);
    }
}
