using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ActorMovementController : MonoBehaviour
{
    [SerializeField] protected Stick moveStick;
    [SerializeField] protected Rigidbody rb;

    protected ActorAttacker attacker;
    protected ActorAnimationController actorAnimationController;
    protected ZCAttributeController zCAttributeController;

    protected float rotateSpeed;
    protected float moveSpeed;
    protected float speedBuffFromZCPower = 0;
    protected float speedBuffFromZCStat = 0;
    protected float speedBuffFromSkin = 0;


    protected Vector3 moveVelocity = Vector3.zero;
    protected Vector3 rotateDir = Vector3.zero;

    public void InitMovementController(ActorAttacker attacker,
        ActorAnimationController actorAnimation, ZCAttributeController zCAttribute)
    {
        rotateSpeed = CONSTANT_VALUE.FIRST_ROTATIONSPEED;
        moveSpeed = CONSTANT_VALUE.FIRST_MOVESPEED;

        this.attacker = attacker;
        actorAnimationController = actorAnimation;
        zCAttributeController = zCAttribute;
        if (moveStick != null)
            moveStick.onThumbstickValueChanged.AddListener(moveStickInputHandler);
        if (zCAttributeController != null)
        {
            zCAttributeController.onChoseZCPower.AddListener(UpdateBuffMovespeedFromZC);
            zCAttributeController.onUpgradeStat.AddListener(OnUpgradeMoveSpeed);
            UpdateBuffMovespeedFromZC();
            OnUpgradeMoveSpeed(zCAttributeController.Stats.FirstOrDefault(stat => stat.Type == Enum.ZCUpgradeType.Speed));
        }

    }    
    //public virtual void Awake()
    //{
    //    rotateSpeed = CONSTANT_VALUE.FIRST_ROTATIONSPEED;
    //    moveSpeed = CONSTANT_VALUE.FIRST_MOVESPEED;
    //}
    //public virtual void OnDisable()
    //{
    //    if (moveStick != null)
    //        moveStick.onThumbstickValueChanged.RemoveListener(moveStickInputHandler);
    //    if (attacker != null)
    //        attacker.onActorStartAttack.RemoveListener(RotateToTarget);
    //}

    public virtual void Move()
    {
        if(GameManager.Instance.CurrentGameState != Enum.GameState.Dead || GameManager.Instance.CurrentGameState != Enum.GameState.Revive || GameManager.Instance.CurrentGameState != Enum.GameState.Win)
        rb.velocity = new Vector3(moveVelocity.x , rb.velocity.y, moveVelocity.z);
        if (rotateDir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(rotateDir);
    }
    public virtual void moveStickInputHandler(Vector2 inputValue)
    {
        float x = inputValue.x;
        float z = inputValue.y;
        
        moveVelocity = new Vector3(x, 0, z).normalized * (moveSpeed + speedBuffFromZCPower + speedBuffFromSkin + speedBuffFromZCStat) * Time.deltaTime;
        rotateDir = inputValue == Vector2.zero ? rotateDir : new Vector3(x, 0, z);
        actorAnimationController.UpdateMoveAnimation(moveVelocity);
    }
    public  virtual void UpdateBuffMovespeedFromZC()
    {
        if(zCAttributeController !=null)
        {
            speedBuffFromZCPower = 0;
            if(zCAttributeController.ZCPower1.PowerType == Enum.ZCPowerUp.MoveFaster)
            {
                speedBuffFromZCPower = 0.2f * moveSpeed;
            }
        }
    }
    public virtual void UpdateBuffFromSkin(float speed)
    {
        speedBuffFromSkin = speed;
    }
    public virtual void OnUpgradeMoveSpeed(ZCStatPlayer zC)
    {
        speedBuffFromZCStat = 0;
        speedBuffFromZCStat = zC.HowMuchUpgrade * moveSpeed / 100;
    }
    
    public virtual void RotateToTarget(GameObject target)
    {
        if (moveVelocity == Vector3.zero && target != null)
        {
            Vector3 roteToDir = target.transform.position - transform.position;
            rotateDir = new Vector3(roteToDir.x, rotateDir.y, roteToDir.z);
        }
    }
}