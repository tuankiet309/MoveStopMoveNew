using UnityEngine;
using UnityEngine.Events;

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

    public delegate void PlayerUpgraded();
    public event PlayerUpgraded onPlayerUpgraded;

    public int Score
    {
        get => score;
        private set { }
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
        {
            attacker.onKillSomeone.AddListener(UpdateScore);
            attacker.onHaveUlti.AddListener(RevertUpdate);
        }
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
        circle.UpdateCircleRadius(CONSTANT_VALUE.CIRCLE_RADIUS_INCREASER);
        if (visualizeCircle != null)
            visualizeCircle.sizeDelta = new Vector2(circle.CircleRadius * 2, circle.CircleRadius * 2);

        scoreMilestone += scoreMilestoneIncreaser;
        scoreMilestoneIncreaser += 1;
        onPlayerUpgraded?.Invoke();
    }

    private float updateTempo = 0;
    private bool isHaveUlti = false;

    public void SetHaveUlti()
    {
        if (!isHaveUlti)
        {
            isHaveUlti = true; 
            TempoUpdate(); 
            attacker.onHaveUlti?.Invoke(true); 
        }
        else
        {
            Debug.Log("Ultimate already activated."); 
        }
    }

    private void TempoUpdate()
    {
        updateTempo = circle.CircleRadius * 0.5f;
        circle.UpdateCircleRadius(updateTempo);
        if (visualizeCircle != null)
            visualizeCircle.sizeDelta = new Vector2(circle.CircleRadius * 2, circle.CircleRadius * 2);
    }

    private void RevertUpdate(bool isStillHaveUlti)
    {
        if (!isStillHaveUlti)
        {
            circle.UpdateCircleRadius(-updateTempo);
            if (visualizeCircle != null)
                visualizeCircle.sizeDelta = new Vector2(circle.CircleRadius * 2, circle.CircleRadius * 2);

            updateTempo = 0; // Reset the update tempo
            isHaveUlti = false; // Reset the ultimate state
        }
    }
}