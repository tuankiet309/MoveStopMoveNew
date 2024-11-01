using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinUIController : MonoBehaviour
{
    [SerializeField] Button PlayZone2;
    [SerializeField] TextMeshProUGUI goldText;

    private void Start()
    {
        PlayZone2.onClick.AddListener(GoToNextZone);
        goldText.text = PlayerGoldInGameController.Instance.Gold.ToString();
        PlayWinningSound();
    }

    private void GoToNextZone()
    {
        PlayerGoldInGameController.Instance.OnEndCurrentLevel();
        LevelManager.Instance.CurrentPVELevel++;
        SceneController.Instance.LoadSceneAsyncWay(Enum.SceneName.PVEScene.ToString());
    }

    private void PlayWinningSound()
    {
        SoundList soundList = SoundManager.Instance.SoundLists.FirstOrDefault(sound => sound.SoundListName == Enum.SoundType.DoneGame);
        AudioClip audioClip = soundList.Sounds[0];
        SoundManager.Instance.PlayThisOnScreen(audioClip, 0.25f);
    }
}
