using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeText : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI text;
    [SerializeField] private float TimeAlive = 2f;
    private float timer = 0f;
    private bool isOpenNow = false;
    private void Start()
    {
        if(Player.Instance != null)
            Player.Instance.GetComponent<ActorAtributeController>().onPlayerUpgraded.AddListener(UpgradeTextNow);
    }
    private void Update()
    {
        if(timer < 0 && isOpenNow)
        {
            gameObject.SetActive(false);
            isOpenNow = false;
            text.text = "";
        }
        timer -= Time.deltaTime;
    }

    private void UpgradeTextNow()
    {
        text.text = "+2.5m";
        isOpenNow = true;
        timer = TimeAlive;
    }


}
