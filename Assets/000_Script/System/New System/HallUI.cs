using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HallUI : MonoBehaviour
{
    public Image img_onSound, img_offSound, img_onVibrate, img_offVibrate;
    public Image img_expImage;
    public TextMeshProUGUI txt_gold, txt_zone;
    public TMP_InputField txt_inputfiledNamePlayer;
    public ShopSkinUI shopSkin;
    public ShopWeaponUI shopWeapon;
    public Animation animatorHallUI;

    public void Init()
    {
        var gameData = DataPersistenceManager.Instance.GameData;

        bool isMute = gameData.settingData.isMute;
        img_offSound.gameObject.SetActive(isMute);
        img_onSound.gameObject.SetActive(!isMute);

        bool isUnvibrate = gameData.settingData.isUnvibrate;
        img_offVibrate.gameObject.SetActive(isUnvibrate);
        img_onVibrate.gameObject.SetActive(!isUnvibrate);

        txt_inputfiledNamePlayer.text = gameData.playerData.playerName;
        txt_gold.text = gameData.gold.ToString();
        txt_zone.text = "Zone: " + gameData.levelData.currentPVELevel + "Rank: " + gameData.maxScore; ;

        shopSkin.InitShopSkin();
        shopWeapon.InitShopWeapon();
    }
}