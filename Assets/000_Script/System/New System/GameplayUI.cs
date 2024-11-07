using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : MonoBehaviour
{

    public Animator animator;

    [Header("____________________________InGame UI_____________________________")]
    public TextMeshProUGUI txt_numberOfEnemyAlive;
    public Stick joystickForPlayer;

    [Header("__________________________Setting UI______________________________")]
    public Image img_onSound;
    public Image img_onVibrate;
    public Image img_offSound;
    public Image img_offVibrate;
    public Button btn_Setting;

    [Header("__________________________Dead UI________________________________")]
    public TextMeshProUGUI txt_rank;
    public TextMeshProUGUI txt_killerName;
    public TextMeshProUGUI txt_deadQuote;
    public Canvas cv_deadUI;

    [Header("__________________________Win UI_________________________________")]
    public Canvas cv_winUI;
    public TextMeshProUGUI txt_winQuote;

    [Header("__________________________Revive UI__________________________")]
    public Canvas cv_reviveUI;
    public TextMeshProUGUI txt_timeLoad;
    public float timeToRevive;
    public Button btn_reviveByGold;
    public Button btn_reviveByAds;

    [Header("_________________________Reward Include__________________")]
    public TextMeshProUGUI txt_gold;
    public Canvas cv_reward;


    public void InitGameplayUI()
    {
        if(GameManager.Instance.dataPersistenceManager.GameData.settingData.isMute)
        {
            img_onSound.gameObject.SetActive(false);
            img_offSound.gameObject.SetActive(true);
        }
        else
        {
            img_onSound.gameObject.SetActive(true);
            img_offSound.gameObject.SetActive(false);
        }
        if(GameManager.Instance.dataPersistenceManager.GameData.settingData.isUnvibrate)
        {
            img_onVibrate.gameObject.SetActive(false);
            img_offVibrate.gameObject.SetActive(true);
        }
        else
        {
            img_onVibrate.gameObject.SetActive(true);
            img_offVibrate.gameObject.SetActive(false);
        }
        txt_numberOfEnemyAlive.text = GameplayManager.Instance.enemySpawner.numberOfEnemiesLeft.ToString();
        btn_Setting.interactable = true;
    }

    public void BTN_Setting()
    {
        animator.SetTrigger("IsOut");
        btn_Setting.interactable = false;
        
    }

}
