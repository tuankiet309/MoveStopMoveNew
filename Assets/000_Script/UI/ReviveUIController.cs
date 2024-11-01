
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;

public class ReviveUIController : MonoBehaviour
{
    [SerializeField] private Transform rotateTime;
    [SerializeField] private TextMeshProUGUI timerText;
    private float countdownTime = 5f;
    private float rotationSpeed = 360f;
    private bool isDenied = false;

    void Start()
    {
        timerText.text = countdownTime.ToString("0");
        StartCoroutine(CountdownCoroutine());
    }
    void Update()
    {
        rotateTime.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }

    private IEnumerator CountdownCoroutine()
    {
        while (countdownTime > 0)
        {
            countdownTime -= 1f;
            countdownTime = Mathf.Max(countdownTime, 0);

            timerText.text = Mathf.Ceil(countdownTime).ToString("0");

            if (countdownTime > 0)
            {
                PlayCountDownSound(0); 
            }
            else
            {
                PlayCountDownSound(2); 
            }

            yield return new WaitForSeconds(1f);
        }

        if (!isDenied)
        {
            isDenied = true;
            GameManager.Instance.SetGameStates(Enum.GameState.Dead, Enum.InGameState.PVE);
        }
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
            Debug.LogWarning("ReviveUIController: Sound or index not found in SoundList.");
        }
    }
}