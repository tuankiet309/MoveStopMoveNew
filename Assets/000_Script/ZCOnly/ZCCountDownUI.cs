using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class ZCCountDownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownNumber;
    [SerializeField] private RectTransform Joystick;
    private float countdownTime = 3f;

    void Start()
    {
        countDownNumber.text = countdownTime.ToString("0");
        StartCoroutine(CountDown());
    }

    private IEnumerator CountDown()
    {
        while (countdownTime > 0)
        {
            countdownTime -= 1f;
            countdownTime = Mathf.Max(countdownTime, 0);

            

            if (countdownTime > 0)
            {
                PlayCountDownSound(0);
                countDownNumber.text = Mathf.Ceil(countdownTime).ToString("0");
            }
            else
            {
                PlayCountDownSound(2);
                countDownNumber.text = "GO";
            }

            yield return new WaitForSeconds(1f); 
        }

        Joystick.gameObject.SetActive(true);
        GameManager.Instance.SetGameStates(Enum.GameState.Begin, Enum.GameplayState.Zombie);
        gameObject.SetActive(false);
    }

    private void PlayCountDownSound(int which)
    {
        SoundList soundList = SoundManager.Instance.SoundLists.FirstOrDefault(sound => sound.SoundListName == Enum.SoundType.CountDown);
        if (soundList != null && which < soundList.Sounds.Length)
        {
            SoundManager.Instance.PlayThisOnScreen(soundList.Sounds[which], 0.3f);
        }
        else
        {
            Debug.LogWarning("ZCCountDownUI: Sound or index not found in SoundList.");
        }
    }
}