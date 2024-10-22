using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZCCountDownUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countDownNumber;
    [SerializeField] RectTransform Joystick;
    private float countdownTime = 3f;
    void Start()
    {
        countDownNumber.text = countdownTime.ToString("0");
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        while (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;

            countdownTime = Mathf.Max(countdownTime, 0);

            countDownNumber.text = Mathf.Ceil(countdownTime).ToString("0");

            yield return new WaitForEndOfFrame();
        }
        Joystick.gameObject.SetActive(true);
        GameManager.Instance.SetGameStates(Enum.GameState.Begin, Enum.InGameState.Zombie);
        gameObject.SetActive(false);

    }
}