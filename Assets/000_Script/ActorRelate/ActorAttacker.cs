using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Linq;

public class ActorAttacker : MonoBehaviour, IAttacker
{
    [SerializeField] protected Transform WeaponHolder;
    [SerializeField] protected Transform throwLocation;
    [SerializeField] protected GameObject targetCircleInstance;

    protected DetectionCircle attackCircle;
    protected WeaponComponent weaponComponent;
    protected ActorAtributeController actorAtributeController;
    protected EnemyMovementController enemyMovementController;
    protected ActorAnimationController actorAnimationController;
    protected ActorMovementController actorMovementController;

    protected HashSet<GameObject> enemyAttackers = new HashSet<GameObject>();
    protected GameObject targetToAttack = null;
    protected Vector3 targetToAttackPos = Vector3.zero;

    protected float distanceBuff = 0;
    protected Weapon weapon;
    protected Projectile weaponToThrow;
    protected GameObject weaponOnHand;

    private bool isUlti = false;
    public UnityEvent<GameObject> onActorStartAttack;

    public void InitAttacker(DetectionCircle detectionCircle,WeaponComponent weaponComponent, 
        ActorAtributeController actorAtributeController, EnemyMovementController enemyMovementController,
        ActorAnimationController actorAnimationController, ActorMovementController actorMovementController)
    {
        attackCircle = detectionCircle;
        this.weaponComponent = weaponComponent;
        this.actorAnimationController = actorAnimationController;
        this.actorAtributeController = actorAtributeController;
        this.enemyMovementController = enemyMovementController;   
        this.actorMovementController = actorMovementController;
        ResetState();
        attackCircle.UpdateCircleRadius(GameManager.Instance.CurrentInGameState == Enum.InGameState.PVE ? CONSTANT_VALUE.FIRST_CIRCLE_RADIUS : CONSTANT_VALUE.ZC_FIRST_CIRCLE_RADIUS);
    }


    public virtual void LateUpdateCheck()
    {
        CheckAndUpdateTargetCircle();
    }
    public virtual void UpdateEnemyList(GameObject target, bool isInCircle)
    {
        if (isInCircle && IsTargetAlive(target))
        {
            if (!enemyAttackers.Contains(target))
            {
                enemyAttackers.Add(target);
            }
            CallEventRelateToHaveTarget(target);
        }
        else
        {
            enemyAttackers.Remove(target);
            if (enemyAttackers.Count == 0)
            {
                CallEventRelateToHaveTarget(null);
            }
        }
    }

    public void CallEventRelateToHaveTarget(GameObject target)
    {
        if (gameObject.CompareTag("Zombie"))
            return;
        actorAnimationController.UpdateHaveTarget(target);
        if (gameObject.CompareTag("Enemy"))
            enemyMovementController.IsTargetInRange(target);
    }

    public virtual void StartAttack()
    {
        actorMovementController.RotateToTarget(GetFirstValidTarget());
    }
    public virtual void CleanUpDestroyedObjects()
    {
        enemyAttackers.RemoveWhere(item => item == null || !item.activeInHierarchy || !IsTargetAlive(item));
    }
    public virtual GameObject GetFirstValidTarget()
    {
        foreach (var target in enemyAttackers)
        {
            if (IsTargetAlive(target))
            {
                return target;
            }
        }
        return null;
    }
    public virtual void CheckAndUpdateTargetCircle()
    {
        CleanUpDestroyedObjects();
        targetToAttack = GetFirstValidTarget();
        if (targetToAttack != null)
        {
            CallEventRelateToHaveTarget(targetToAttack);
            targetToAttackPos = targetToAttack.transform.position;
            if (targetCircleInstance != null)
            {
                targetCircleInstance.SetActive(true);
                targetCircleInstance.transform.position = new Vector3(targetToAttackPos.x, targetCircleInstance.transform.position.y, targetToAttackPos.z);
                targetCircleInstance.transform.rotation = Quaternion.Euler(90,0,0);
            }
        }
        else
        {
            CallEventRelateToHaveTarget(null);
            if (targetCircleInstance != null)
            {
                targetCircleInstance.SetActive(false);
            }
        }
    }

    public virtual bool IsTargetAlive(GameObject target)
    {
        LifeComponent deadController = target.GetComponent<LifeComponent>();
        return deadController != null && !deadController.IsDead;
    }
    public virtual void Attack(Vector3 enemyLoc)
    {
        Vector3 throwDirection = enemyLoc - throwLocation.position;
        throwDirection.y = 0;
        Quaternion throwRotation = Quaternion.LookRotation(throwDirection);
        Projectile newProjectile = Instantiate(weaponToThrow, throwLocation.position,throwRotation);
        newProjectile.gameObject.SetActive(true);
        newProjectile.InitForProjectileToThrow(this, weapon.WeaponType, distanceBuff +  (actorAtributeController.BuffValues.ContainsKey(Enum.AttributeBuffs.Range)  ? actorAtributeController.BuffValues[Enum.AttributeBuffs.Range] : 0));
        newProjectile.FlyToPos(enemyLoc, isUlti);
        if (isUlti)
        {
            isUlti = false;
            actorAnimationController.UpdateHaveUlti(false);
        }
    }
    public virtual void Attack(Vector3 enemyLoc, Vector3 throwLocationTemp, bool isMainAttack)
    {
    }
    public void CallEventIfKillSomeOne()
    {
        if (gameObject.CompareTag("Zombie"))
            return;
        actorAtributeController.UpdateScore();
    }

    public virtual void PrepareToAttack()
    {
        CallEventRelateToAttack();
        Attack(targetToAttackPos);
        targetToAttackPos = Vector3.zero;
    }

    public virtual void CallEventRelateToAttack()
    {
        if (gameObject.CompareTag("Zombie"))
            return;
        Vector3 attackDir = targetToAttackPos - transform.position;
        if (gameObject.CompareTag("Enemy"))
        {
            enemyMovementController.IsAttackingRightNow(new Vector2(attackDir.x, attackDir.z));
        }
        weaponComponent.OnThrowAwayWeapon();
    }

    public virtual void InitWeapon(Weapon newWeapon)
    {
        ClearOldWeapon();
        weapon = newWeapon;
        weaponOnHand = Instantiate(weapon.WeaponOnHand, WeaponHolder);
        if (gameObject.CompareTag("Player"))
        {
            weaponComponent.ApplyWeaponSkin(weaponOnHand, weapon.CurrentIndexOfTheSkin);
        }
        else
        {
            weaponComponent.ApplyWeaponSkin(weaponOnHand, weapon.TempoIndex);
        }
        weaponToThrow = Instantiate(weapon.WeaponThrowAway, transform);
        GameObject visualize = Instantiate(weaponOnHand, this.weaponToThrow.transform);
        weaponOnHand.transform.localPosition = weapon.WeaponOffsetPos;
        weaponOnHand.transform.localRotation = weapon.WeaponOffsetRot;
        weaponToThrow.transform.rotation = Quaternion.Euler(Vector3.zero);
        weaponToThrow.gameObject.SetActive(false);
        visualize.transform.localPosition = weapon.WeaponOffsetOnThrow;
    }
    public void ClearOldWeapon()
    {
        if(weaponOnHand!=null)
        Destroy(weaponOnHand.gameObject);
        if(weaponToThrow!=null)
        Destroy(weaponToThrow.gameObject);
    }

    public void ResetState()
    {
        enemyAttackers.Clear();
        CallEventRelateToHaveTarget(null);
        targetToAttack = null;
        targetToAttackPos = Vector3.zero;
    }
    public void SetUlti()
    {
        if (!isUlti)
        {
            isUlti = true;
            actorAnimationController.UpdateHaveUlti(true);
        }
    }
    public virtual void UpgradeFromSkin(float distance)
    {
        distanceBuff += distance;
    }
    public virtual void UpgradePlayer()
    {
        distanceBuff += CONSTANT_VALUE.CIRCLE_RADIUS_INCREASER + 0.5f;
        UpgradeCircle(1);
    }
    public virtual void UpgradeCircle(int multiplier)
    {
        attackCircle.UpdateCircleRadius(CONSTANT_VALUE.CIRCLE_RADIUS_INCREASER * multiplier);
    }
    public virtual void PlayAttackSound()
    {
        SoundList sound = SoundManager.Instance.SoundLists.FirstOrDefault(soundList => soundList.SoundListName == Enum.SoundType.WeaponSound);
        Debug.Log(sound.SoundListName);
        AudioClip audioToPlay = sound.Sounds[1];
        SoundManager.Instance.PlayThisOnWorld(audioToPlay, 1f, throwLocation.position);
    }
  
}