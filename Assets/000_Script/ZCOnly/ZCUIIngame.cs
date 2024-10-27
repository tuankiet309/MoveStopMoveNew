using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZCUIIngame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI aliveText;
    [SerializeField] private TextMeshProUGUI Day;
    [SerializeField] private Button settingButton;
    private void Start()
    {
        if (ZombieSpawner.Instance != null)
        {
            ZombieSpawner.Instance.OnNumberOfEnemiesDecrease.AddListener(UpdateAlive);
            UpdateAlive(ZombieSpawner.Instance.NumberOfEnemiesLeft);
        }
        Day.text = "Day " +(LevelManager.Instance.CurrentZCLevel + 1).ToString();
    }
    private void UpdateAlive(int number)
    {
        aliveText.text = number.ToString();
    }

}
