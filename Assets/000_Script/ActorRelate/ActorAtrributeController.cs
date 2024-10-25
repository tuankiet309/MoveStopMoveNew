using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActorAtributeController : MonoBehaviour,IDataPersistence
{
    protected int score;
    protected int scoreMilestone;
    protected int scoreMilestoneIncreaser;
    protected float bodyScalerIncreaser;

    [SerializeField] protected DetectionCircle circle;
    [SerializeField] protected ActorAttacker attacker;
    [SerializeField] protected Transform playerVisualize;
    [SerializeField] protected RectTransform visualizeCircle;

    [SerializeField] protected SkinComponent skinComponent;
    [SerializeField] protected WeaponComponent weaponComponent;

    private Dictionary<Enum.AttributeBuffs, float> buffValues = new Dictionary<Enum.AttributeBuffs, float>();


    public UnityEvent onScoreChanged;

    public UnityEvent onPlayerUpgraded;

    public UnityEvent onBuffChange;

    public int Score
    {
        get => score;
        protected set { }
    }

    public Dictionary<Enum.AttributeBuffs, float> BuffValues { get => buffValues; set => buffValues = value; }

    protected virtual void Awake()
    {
        scoreMilestone = CONSTANT_VALUE.FIRST_SCORE_MILESTONE;
        scoreMilestoneIncreaser = CONSTANT_VALUE.SCORE_MILESTONE_INCREASER;
        bodyScalerIncreaser = CONSTANT_VALUE.BODY_SCALER_INCREASER;
    }

    protected virtual void OnEnable()
    {
        if (attacker != null)
        {
            attacker.onKillSomeone.AddListener(UpdateScore);
            attacker.onHaveUlti.AddListener(RevertUpdate);
        }

        if (weaponComponent != null) 
        {
            weaponComponent.onAssignNewWeapon.AddListener(ApplyBuffByWeapon);
        }
        if(skinComponent != null)
        {
            skinComponent.onWearNewSkin.AddListener(ApplyBuffBySkin);
        }
    }


    private void ApplyBuffByWeapon(Weapon oldWeapon, Weapon newWeapon)
    {
        if (oldWeapon != null && oldWeapon.Buff == Enum.AttributeBuffs.Range)
        {
            if (buffValues.ContainsKey(oldWeapon.Buff))
            {
                buffValues[oldWeapon.Buff] -= oldWeapon.BuffMultiplyer;
            }
        }

        if (newWeapon.Buff == Enum.AttributeBuffs.Range)
        {
            if (buffValues.ContainsKey(newWeapon.Buff))
            {
                buffValues[newWeapon.Buff] += newWeapon.BuffMultiplyer;
            }
            else
            {
                buffValues[newWeapon.Buff] = newWeapon.BuffMultiplyer;
            }
        }
        onBuffChange?.Invoke();
    }
    private void ApplyBuffBySkin(List<Skin> oldSkin, List<Skin> newSkin)
    {
        if (oldSkin != null)
        {
            foreach (Skin skin in oldSkin)
            {
                if (buffValues.ContainsKey(skin.AttributeBuffs))
                {
                    buffValues[skin.AttributeBuffs] -= skin.BuffMultiplyer;
                }
            }
        }

        foreach (Skin skin in newSkin)
        {
            if (buffValues.ContainsKey(skin.AttributeBuffs))
            {
                buffValues[skin.AttributeBuffs] += skin.BuffMultiplyer;
            }
            else
            {
                buffValues[skin.AttributeBuffs] = skin.BuffMultiplyer;
            }
        }
        onBuffChange?.Invoke();

    }

    protected virtual void Start()
    {
        if (visualizeCircle != null)
            visualizeCircle.sizeDelta = new Vector2(circle.CircleRadius * 2, circle.CircleRadius * 2);
    }

    protected virtual void OnDisable()
    {
        if (attacker != null)
            attacker.onKillSomeone.RemoveListener(UpdateScore);
    }

    protected virtual void UpdateScore()
    {
        score++;
        onScoreChanged?.Invoke();
        CheckForUpgrade();
    }

    protected virtual void CheckForUpgrade()
    {
        if (score >= scoreMilestone)
        {
            UpgradePlayer();
            scoreMilestone += scoreMilestoneIncreaser;
            scoreMilestoneIncreaser += 1;
        }
    }

    protected virtual void UpgradePlayer()
    {
        playerVisualize.localScale += new Vector3(bodyScalerIncreaser, bodyScalerIncreaser, bodyScalerIncreaser);
        circle.UpdateCircleRadius(CONSTANT_VALUE.CIRCLE_RADIUS_INCREASER);
        if (visualizeCircle != null)
            visualizeCircle.sizeDelta = new Vector2(circle.CircleRadius * 2, circle.CircleRadius * 2);
        onPlayerUpgraded?.Invoke();
    }

    protected float updateTempo = 0;
    protected bool isHaveUlti = false;

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

    protected virtual void TempoUpdate()
    {
        updateTempo = circle.CircleRadius * 0.5f;
        circle.UpdateCircleRadius(updateTempo);
        if (visualizeCircle != null)
            visualizeCircle.sizeDelta = new Vector2(circle.CircleRadius * 2, circle.CircleRadius * 2);
    }

    protected virtual void RevertUpdate(bool isStillHaveUlti)
    {
        if (!isStillHaveUlti)
        {
            circle.UpdateCircleRadius(-updateTempo);
            if (visualizeCircle != null)
                visualizeCircle.sizeDelta = new Vector2(circle.CircleRadius * 2, circle.CircleRadius * 2);

            updateTempo = 0;
            isHaveUlti = false;
        }
    }

    public void LoadData(GameData gameData)
    {
        
    }

    public void SaveData(ref GameData gameData)
    {
        if(score > gameData.maxScore)
        {
            gameData.maxScore = score;
        }
    }
}