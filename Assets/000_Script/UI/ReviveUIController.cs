using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReviveUIController : MonoBehaviour
{
    [SerializeField] Transform rotateTime;  
    [SerializeField] TextMeshProUGUI timerText;  

    private float countdownTime = 5f; 
    private float rotationSpeed = 360f;  

    private bool isDenied = false;

    void Start()
    {
        timerText.text = countdownTime.ToString("0");
    }

    void Update()
    {
        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;

            countdownTime = Mathf.Max(countdownTime, 0);

            timerText.text = Mathf.Ceil(countdownTime).ToString("0");

            rotateTime.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        }
        else if (!isDenied) 
        {
            isDenied = true;
            GameManager.Instance.SetGameStates(Enum.GameState.Dead, Enum.InGameState.PVE);
        }
    }
}
