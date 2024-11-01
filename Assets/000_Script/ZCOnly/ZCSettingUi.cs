using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZCSettingUi : MonoBehaviour
{
    [SerializeField] RectTransform textNoAbility;
    [SerializeField] RectTransform abilityInfomation;
    [SerializeField] Image abilityImage;
    [SerializeField] TextMeshProUGUI abilityName;
    [SerializeField] TextMeshProUGUI abilityDes;

    private void Start()
    {
        ZCPower zcPower = Player.Instance.GetComponent<ZCAttributeController>().ZCPower1;
        if (zcPower != null) 
        {
            textNoAbility.gameObject.SetActive(false);
            abilityInfomation.gameObject.SetActive(true);
            abilityImage.sprite = zcPower.PowerImage;
            abilityName.text = zcPower.PowerName;
            abilityDes.text = zcPower.PowerDes;
        }
    }
}
