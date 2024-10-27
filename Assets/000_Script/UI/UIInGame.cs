using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI aliveText;
    [SerializeField] private TextMeshProUGUI fpsText;
    [SerializeField] private Button settingButton;


    private void Start()
    {
        if (EnemySpawner.Instance != null)
        {
            EnemySpawner.Instance.OnNumberOfEnemiesDecrease.AddListener(UpdateAlive);
            UpdateAlive(EnemySpawner.Instance.NumberOfEnemiesLeft);
        }
        StartCoroutine(DoFPS());
    }

    private void UpdateAlive(int number)
    {
        aliveText.text ="Alive: "  +number.ToString();
    }

    IEnumerator DoFPS()
    {
        while (true)
        {
            fpsText.text = (1 / Time.deltaTime).ToString();
            yield return new WaitForSeconds(0.25f);
        }
    }
}