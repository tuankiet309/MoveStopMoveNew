using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ActorAtributeController : MonoBehaviour
{
    private int score;
    private int scoreMilestone;
    private int scoreMilestoneIncreaser;
    private float bodyScalerIncreaser;

    [SerializeField] private DetectionCircle circle;
    [SerializeField] private ActorAttacker attacker;
    [SerializeField] private Transform playerVisualize;
    [SerializeField] private RectTransform visualizeCircle;

    public delegate void ScoreChanged();
    public event ScoreChanged onScoreChanged;

    public int Score
    {
        get => score;
        private set
        {
             
        }
    }

    private void Awake()
    {
        scoreMilestone = CONSTANT_VALUE.FIRST_SCORE_MILESTONE;
        scoreMilestoneIncreaser = CONSTANT_VALUE.SCORE_MILESTONE_INCREASER;
        bodyScalerIncreaser = CONSTANT_VALUE.BODY_SCALER_INCREASER;
    }

    private void OnEnable()
    {
        if (attacker != null)
            attacker.onKillSomeone.AddListener(UpdateScore);
        

    }
    private void Start()
    {
        if (visualizeCircle != null)
            visualizeCircle.sizeDelta = new Vector2(circle.CircleRadius * 2, circle.CircleRadius * 2);
    }
    private void OnDisable()
    {
        if (attacker != null)
            attacker.onKillSomeone.RemoveListener(UpdateScore); 
    }

    private void UpdateScore()
    {
        score++;
        onScoreChanged?.Invoke();
        CheckForUpgrade();
    }
    private void CheckForUpgrade()
    {
        if (score >= scoreMilestone)
        {
            UpgradePlayer(); 
        }
    }

    private void UpgradePlayer()
    {
        playerVisualize.localScale += new Vector3(bodyScalerIncreaser, bodyScalerIncreaser, bodyScalerIncreaser);

        circle.UpdateCircleRadius();
        if(visualizeCircle != null)
            visualizeCircle.sizeDelta = new Vector2(circle.CircleRadius * 2, circle.CircleRadius * 2);
        scoreMilestone += scoreMilestoneIncreaser;
        scoreMilestoneIncreaser += 1;

    }
}