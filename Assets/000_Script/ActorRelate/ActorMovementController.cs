using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ActorMovementController : MonoBehaviour
{
    [SerializeField] protected Stick moveStick;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected ActorAttacker attacker;
    [SerializeField] protected ActorAtributeController actorAtributeController;
    protected float rotateSpeed;
    protected float moveSpeed;
    protected float speedBuffFromZCPower = 0;
    protected float speedBuffFromZCStat = 0;
    protected float speedBuffFromSkin = 0;

    protected Vector3 moveVelocity = Vector3.zero;
    protected Vector3 rotateDir = Vector3.zero;

    public UnityEvent<Vector3> onActorMoving;

    protected ZCAttributeController ZCAttributeController;
    protected virtual void Awake()
    {
        rotateSpeed = CONSTANT_VALUE.FIRST_ROTATIONSPEED;
        moveSpeed = CONSTANT_VALUE.FIRST_MOVESPEED;
    }

    protected virtual void OnEnable()
    {
        if (moveStick != null)
            moveStick.onThumbstickValueChanged.AddListener(moveStickInputHandler);
        if (attacker != null)
            attacker.onActorStartAttack.AddListener(RotateToTarget);
        if(actorAtributeController != null)
        {
            actorAtributeController.onBuffChange.AddListener(UpdateBuffFromSkin);
            ZCAttributeController = actorAtributeController as ZCAttributeController;
            if(ZCAttributeController != null)
            {
                ZCAttributeController.onChoseZCPower.AddListener(UpdateBuffMovespeedFromZC);
                ZCAttributeController.onUpgradeStat.AddListener(OnUpgradeMoveSpeed);
                UpdateBuffMovespeedFromZC();
                OnUpgradeMoveSpeed(ZCAttributeController.Stats.FirstOrDefault(stat => stat.Type == Enum.ZCUpgradeType.Speed));
            }
        }
 
    }

    protected virtual void OnDisable()
    {
        if (moveStick != null)
            moveStick.onThumbstickValueChanged.RemoveListener(moveStickInputHandler);
        if (attacker != null)
            attacker.onActorStartAttack.RemoveListener(RotateToTarget);
        if (actorAtributeController != null)
        {
            actorAtributeController.onBuffChange.RemoveListener(UpdateBuffFromSkin);
        }
    }

    protected virtual void Update()
    {
        if(GameManager.Instance.CurrentGameState != Enum.GameState.Dead || GameManager.Instance.CurrentGameState != Enum.GameState.Revive || GameManager.Instance.CurrentGameState != Enum.GameState.Win)
        rb.velocity = new Vector3(moveVelocity.x , rb.velocity.y, moveVelocity.z);
        if (rotateDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(rotateDir);
    }
    
    protected virtual void moveStickInputHandler(Vector2 inputValue)
    {
        float x = inputValue.x;
        float z = inputValue.y;
        
        moveVelocity = new Vector3(x, 0, z).normalized * (moveSpeed + speedBuffFromZCPower + speedBuffFromSkin + speedBuffFromZCStat) * Time.deltaTime;
        rotateDir = inputValue == Vector2.zero ? rotateDir : new Vector3(x, 0, z);
        
        onActorMoving?.Invoke(moveVelocity);
    }
    protected virtual void UpdateBuffMovespeedFromZC()
    {
        if(ZCAttributeController !=null)
        {
            speedBuffFromZCPower = 0;
            if(ZCAttributeController.ZCPower1.PowerType == Enum.ZCPowerUp.MoveFaster)
            {
                speedBuffFromZCPower = 0.2f * moveSpeed;
            }
        }
        
    }
    protected virtual void UpdateBuffFromSkin()
    {
        if (actorAtributeController.BuffValues.ContainsKey(Enum.AttributeBuffs.Speed))
        {
            speedBuffFromSkin = actorAtributeController.BuffValues[Enum.AttributeBuffs.Speed];
        }
    }
    protected virtual void OnUpgradeMoveSpeed(ZCStatPlayer zC)
    {
        speedBuffFromZCStat = 0;
        speedBuffFromZCStat = zC.HowMuchUpgrade * moveSpeed / 100;
    }
    
    protected virtual void RotateToTarget(GameObject target)
    {
        if (moveVelocity == Vector3.zero && target != null)
        {
            Vector3 roteToDir = target.transform.position - transform.position;
            rotateDir = new Vector3(roteToDir.x, rotateDir.y, roteToDir.z);
        }
    }
}