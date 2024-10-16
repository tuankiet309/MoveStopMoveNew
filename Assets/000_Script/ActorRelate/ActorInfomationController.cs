using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActorInformationController : MonoBehaviour
{
    // Components to manage the UI
    [SerializeField] private ActorAtributeController attributesController; 
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void OnEnable()
    {
        if (attributesController != null)
            attributesController.onScoreChanged += UpdateScoreDisplay; 
    }

    private void OnDisable()
    {
        if (attributesController != null)
            attributesController.onScoreChanged -= UpdateScoreDisplay; 
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
        nameText.text = name; 
    }

    internal string GetName()
    {
        return nameText.text;
    }
    internal int GetScore()
    {
        return int.Parse(scoreText.text);
    }
}