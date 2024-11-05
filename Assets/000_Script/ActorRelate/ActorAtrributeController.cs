using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActorAtributeController : MonoBehaviour,IDataPersistence
{
    protected int score;
    protected int scoreMilestone;
    protected int scoreMilestoneIncreaser;
    protected float bodyScalerIncreaser;
    [SerializeField] protected Transform playerVisualize;

    protected ActorAttacker attacker;
    protected ActorMovementController movementController;
    protected ActorInformationController actorInformationController;


    private Dictionary<Enum.AttributeBuffs, float> buffValues = new Dictionary<Enum.AttributeBuffs, float>();
    public UnityEvent onPlayerUpgraded;
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
    public void InitAttribute(ActorAttacker attacker, ActorMovementController actorMovement, ActorInformationController actorInformation)
    { 
        this.attacker = attacker;
        this.movementController = actorMovement;
        this.actorInformationController = actorInformation;
    }
    public void ApplyBuffByWeapon(Weapon oldWeapon, Weapon newWeapon)
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
        if (gameObject.CompareTag("Player"))
        {
            if (buffValues.ContainsKey(Enum.AttributeBuffs.Range))
                attacker.UpgradeFromSkin(buffValues[Enum.AttributeBuffs.Range]);
        }
    }
    public void ApplyBuffBySkin(List<Skin> oldSkin, List<Skin> newSkin)
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
        if (gameObject.CompareTag("Player"))
        {
            if (buffValues.ContainsKey(Enum.AttributeBuffs.Speed))
                movementController.UpdateBuffFromSkin(buffValues[Enum.AttributeBuffs.Speed]);
        }
    }

    public virtual void UpdateScore()
    {
        score++;        
        CallEVentIfScoreChange();
        CheckForUpgrade();
    }
    public void CallEVentIfScoreChange()
    {
        actorInformationController.UpdateScoreDisplay(score);
        if (gameObject.CompareTag("Player"))
        {
            DataPersistenceManager.Instance.GameData.currentExp++;
            PlayerGoldInGameController.Instance.UpdateGold();
        }
        if (gameObject.CompareTag("Enemy"))
        {
            GetComponent<Target>().UpdateScore(score);
        }
    }
    protected virtual void CheckForUpgrade()
    {
        if (score >= scoreMilestone)
        {
            UpgradePlayer();
            attacker.UpgradePlayer();
            if(gameObject.CompareTag("Player"))
                CameraController.Instance.AdjustCameraDistance();
            scoreMilestone += scoreMilestoneIncreaser;
            scoreMilestoneIncreaser += 1;
        }
    }
    protected virtual void UpgradePlayer()
    {
        playerVisualize.localScale += new Vector3(bodyScalerIncreaser, bodyScalerIncreaser, bodyScalerIncreaser);
        if(gameObject.CompareTag("Player"))
        {
            SoundManager.Instance.Vibrate();
        }
    }
    protected float updateTempo = 0;
    protected bool isHaveUlti = false;

    public void SetHaveUlti()
    {
        if (!isHaveUlti)
        {
            isHaveUlti = true;
            attacker.SetUlti();
        }
        else
        {
            Debug.Log("Ultimate already activated.");
        }
    }

    public virtual void LoadData(GameData gameData)
    {
        
    }
    public virtual void SaveData(ref GameData gameData)
    {
        if(score > gameData.maxScore)
        {
            gameData.maxScore = score;
        }
    }
}