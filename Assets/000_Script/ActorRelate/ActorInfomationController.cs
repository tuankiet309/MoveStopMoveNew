using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActorInformationController : MonoBehaviour,IDataPersistence
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private string actorName;

    public string Name { get => actorName; set => actorName = value; }

    public void UpdateScoreDisplay(int score)
    {
        if (gameObject.CompareTag("Zombie"))
            return; 
        scoreText.text = score.ToString(); 
    }

    public void UpdateName(string name)
    {
        this.actorName = name;
        nameText.text = this.actorName; 
    }

    public string GetName()
    {
        return actorName;
    }
    public int GetScore()
    {
        return int.Parse(scoreText.text);
    }

    public void LoadData(GameData gameData)
    {
        UpdateName(gameData.playerData.playerName);
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.playerData.playerName = this.actorName;
    }
}