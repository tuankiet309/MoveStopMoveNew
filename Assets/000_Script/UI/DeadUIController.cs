using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeadUIController : MonoBehaviour
{
    [SerializeField]
    string[] empathysForRhapsody;
    [SerializeField] private TextMeshProUGUI empathyText;
    [SerializeField] private TextMeshProUGUI rankedText;
    [SerializeField] private TextMeshProUGUI KillerText;
    [SerializeField] private Button continueBtn;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private RectTransform reviveHolder;
    [SerializeField] private Button reviveButton;


    private float originalSize;

    private void Start()
    {
        //continueBtn.onClick.AddListener(OnContinueClick);
        //rankedText.text = (EnemySpawner.Instance.NumberOfEnemiesLeft + 1).ToString();
        //KillerText.text = Player.Instance.GetComponent<LifeComponent>().KillerName;
        //int score = Player.Instance.GetComponent<ActorInformationController>().GetScore();
        //int randomEmpathy = Random.Range(0,empathysForRhapsody.Length);
        //empathyText.text = empathysForRhapsody[randomEmpathy];
        //originalSize = reviveHolder.sizeDelta.x;
        //if(score < 5)
        //{
        //    reviveHolder.sizeDelta = new Vector2(reviveHolder.sizeDelta.x / 3, reviveHolder.sizeDelta.y);
        //    reviveButton.gameObject.SetActive(false);
        //}
        //else
        //{
        //    reviveHolder.sizeDelta = new Vector2(originalSize, reviveHolder.sizeDelta.y);
        //    reviveButton.gameObject.SetActive(true);
        //    reviveButton.onClick.AddListener(ClaimGoldX3AndRestart);
        //}
        //gold.text = PlayerGoldInGameController.Instance.Gold.ToString();
        //PlayLosingSound();
        //DataPersistenceManager.Instance.SaveGame();
    }
   
    private void OnContinueClick()
    {
        PlayerGoldInGameController.Instance.OnEndCurrentLevel();
        SceneController.Instance.LoadSceneAsyncWay(Enum.SceneName.PVEScene.ToString());
    }
    private void PlayLosingSound()
    {
        SoundList soundList = SoundManager.Instance.SoundLists.FirstOrDefault(sound => sound.SoundListName == Enum.SoundType.DoneGame);
        AudioClip audioClip = soundList.Sounds[1];
        SoundManager.Instance.PlayThisOnScreen(audioClip, 0.5f);
    }
    private void ClaimGoldX3AndRestart()
    {
        PlayerGoldInGameController.Instance.Gold *= 3;
        PlayerGoldInGameController.Instance.OnEndCurrentLevel();
        SceneController.Instance.LoadSceneAsyncWay(Enum.SceneName.PVEScene.ToString());
    }
}
