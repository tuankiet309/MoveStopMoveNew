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
    [SerializeField] private TextMeshProUGUI goldText;
    private void Start()
    {
        if (ZombieSpawner.Instance != null)
        {
            ZombieSpawner.Instance.OnNumberOfEnemiesDecrease.AddListener(UpdateAlive);
            UpdateAlive(ZombieSpawner.Instance.NumberOfEnemiesLeft);
        }

        Day.text = "Day " + (LevelManager.Instance.CurrentZCLevel + 1).ToString();
        if (DataPersistenceManager.Instance != null)
        {
            goldText.text = DataPersistenceManager.Instance.GameData.gold.ToString();
            DataPersistenceManager.Instance.OnGoldChange.AddListener(UpdateGoldText);
        }
    }
    private void UpdateAlive(int number)
    {
        aliveText.text = number.ToString();
    }

    private void UpdateGoldText()
    {
        goldText.text = DataPersistenceManager.Instance.GameData.gold.ToString();

    }

}
