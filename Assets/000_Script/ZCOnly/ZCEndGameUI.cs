using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ZCEndGameUI : MonoBehaviour
{
    [SerializeField] private RectTransform daysHolder;
    [SerializeField] private RectTransform starHolder;
    [SerializeField] private RectTransform numberStarHolder;

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI gold;
    [SerializeField] private TextMeshProUGUI goldx3;

    [SerializeField] private Button claim;
    [SerializeField] private Button claimX3;
    [SerializeField] private Button homeButton;

    private void Start()
    {
        if(GameManager.Instance.CurrentGameState == Enum.GameState.Win)
        {
            Won();
        }
        else
        {
            Lose();
        }
        gold.text = PlayerGoldInGameController.Instance.Gold.ToString();
        goldx3.text = (PlayerGoldInGameController.Instance.Gold*3).ToString();
        claim.onClick.AddListener(ClaimGoldAndRestart);
        claimX3.onClick.AddListener (ClaimGoldX3AndRestart);
        homeButton.onClick.AddListener(GoToHome);
        SceneController.Instance.LoadSceneAsyncWay(SceneManager.GetSceneByBuildIndex(1));
    }
    private void GoToHome()
    {
        SceneController.Instance.LoadSceneRightAway(SceneManager.GetSceneByBuildIndex(0));
    }
    private void ClaimGoldAndRestart()
    {
        PlayerGoldInGameController.Instance.OnEndCurrentLevel();
        SceneController.Instance.AddThisEventToActiveScene();
    }
    private void ClaimGoldX3AndRestart()
    {
        PlayerGoldInGameController.Instance.Gold *= 3;
        PlayerGoldInGameController.Instance.OnEndCurrentLevel();
        SceneController.Instance.AddThisEventToActiveScene();
    }
    private void Won()
    {
        title.text = "You survive day " + (LevelManager.Instance.CurrentZCLevel + 1).ToString();
        starHolder.gameObject.SetActive(true);
        numberStarHolder.gameObject.SetActive(true);
        int index = LevelManager.Instance.CurrentZCLevel % 5 +1;
        for (int i = 0; i < index; i++)
        {
            daysHolder.GetChild(i).GetChild(0).gameObject.SetActive(true);
            daysHolder.GetChild(i).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Day " + (LevelManager.Instance.CurrentZCLevel - index  ).ToString();
            if (i == index)
            {
                daysHolder.GetChild(i).GetChild(1).gameObject.SetActive(true);
            }
        }
        LevelManager.Instance.CurrentZCLevel++;
    }
    private void Lose()
    {
        title.text = "You lose";
        starHolder.gameObject.SetActive(false);
        numberStarHolder.gameObject.SetActive(false);
        int index = LevelManager.Instance.CurrentZCLevel % 5;
        for (int i = 0; i < index; i++)
        {
            daysHolder.GetChild(i).GetChild(0).gameObject.SetActive(true);
            daysHolder.GetChild(i).GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Day " + (LevelManager.Instance.CurrentZCLevel - index).ToString();
            if (i == index)
            {
                daysHolder.GetChild(i).GetChild(1).gameObject.SetActive(true);
                daysHolder.GetChild(i).GetChild(1).GetComponent<Image>().color = new Color(1, 0, 0, 1);
            }
        }
    }
}
