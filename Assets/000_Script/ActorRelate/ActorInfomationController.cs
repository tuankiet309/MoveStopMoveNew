using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActorInformationController : MonoBehaviour,IDataPersistence
{
    // Components to manage the UI
    [SerializeField] private ActorAtributeController attributesController; 
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private string actorName;

    public string Name { get => actorName; set => actorName = value; }

    private void OnEnable()
    {
        if (attributesController != null)
            attributesController.onScoreChanged.AddListener(UpdateScoreDisplay); 
    }

    private void OnDisable()
    {
        if (attributesController != null)
            attributesController.onScoreChanged.RemoveListener(UpdateScoreDisplay); 
    }

    private void Start()
    {
        UpdateScoreDisplay(); 
    }

    private void UpdateScoreDisplay()
    {
        if (attributesController != null)
        {
            scoreText.text = attributesController.Score.ToString(); 
        }
    }

    public void UpdateName(string name)
    {
        this.actorName = name;
        nameText.text = this.actorName; 
    }

    internal string GetName()
    {
        return "Hahah";
    }
    internal int GetScore()
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