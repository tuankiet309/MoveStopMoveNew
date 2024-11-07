using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float timeAlive = 2f;
    [SerializeField] private float floatSpeed = 20f; // Controls how fast the text moves up
    private float timer = 0f;
    private bool isShowing = false;
    private Vector3 initialPosition;

    private void Start()
    {

        if (Player.Instance != null && GameManager.Instance.CurrentInGameState !=Enum.GameplayState.Zombie)
            Player.Instance.GetComponent<ActorAtributeController>().onPlayerUpgraded.AddListener(ShowUpgradeText);
        initialPosition = text.rectTransform.localPosition;
    }

    private void Update()
    {
        if (isShowing)
        {
            timer -= Time.deltaTime;
 
            text.rectTransform.localPosition += Vector3.up * floatSpeed * Time.deltaTime;

            if (timer <= 0)
            {
                text.text = "";
                text.rectTransform.localPosition = initialPosition;
                isShowing = false;
            }
        }
    }

    public void ShowUpgradeText()
    {
        text.text = "+2.5m"; 
        isShowing = true;
        timer = timeAlive;
        text.rectTransform.localPosition = initialPosition; 
    }
}