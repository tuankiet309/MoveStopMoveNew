using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class ActorAttacker : MonoBehaviour, IAttacker
{
    [SerializeField] protected DetectionCircle attackCircle;
    [SerializeField] protected Transform throwLocation;
    [SerializeField] protected GameObject targetCircleInstance;
    [SerializeField] protected WeaponComponent weaponComponent;
    [SerializeField] protected ActorAtributeController actorAtributeController;
    [SerializeField] protected Transform WeaponHolder;

    protected HashSet<GameObject> enemyAttackers = new HashSet<GameObject>();
    protected GameObject targetToAttack = null;
    protected Vector3 targetToAttackPos = Vector3.zero;

    protected float distanceBuff = 0;

    protected Weapon weapon;
    protected Projectile weaponToThrow;
    protected GameObject weaponOnHand;

    private bool isUlti = false;

    
    
    public UnityEvent<Vector2> onActorAttack;
    public UnityEvent<GameObject> onHaveTarget;
    public UnityEvent<bool> onHaveUlti;
    public UnityEvent onKillSomeone;

    protected virtual void OnEnable()
    {
        if (attackCircle != null)
        {
            attackCircle.onTriggerContact.AddListener(UpdateEnemyList);
        }
        if (weaponComponent != null) 
        {
            weaponComponent.onAssignNewWeapon.AddListener(InitWeapon);
        }
        if(actorAtributeController != null)
        {
            actorAtributeController.onPlayerUpgraded.AddListener(OnUpgrade);
        }
        ResetState();
        onHaveUlti.AddListener(SetUlti);
        
    }
    protected virtual void OnDisable()
    {
        if (attackCircle != null)
        {
            attackCircle.onTriggerContact.RemoveListener(UpdateEnemyList);
        }
        if (weaponComponent != null)
        {
            weaponComponent.onAssignNewWeapon.RemoveListener(InitWeapon);
        }
        if (actorAtributeController != null)
        {
            actorAtributeController.onPlayerUpgraded.RemoveListener(OnUpgrade);

        }
        ResetState();
    }
    protected virtual void Start()
    {

    }
    protected virtual void Update()
    {
        CheckAndUpdateTargetCircle();
    }
    protected virtual void UpdateEnemyList(GameObject target, bool isInCircle)
    {
        if (isInCircle && IsTargetAlive(target))
        {
            if (!enemyAttackers.Contains(target))
            {
                enemyAttackers.Add(target);
            }
            onHaveTarget?.Invoke(target);
        }
        else
        {
            enemyAttackers.Remove(target);
            if (enemyAttackers.Count == 0)
                onHaveTarget?.Invoke(null);
        }
    }
    protected virtual void CleanUpDestroyedObjects()
    {
        enemyAttackers.RemoveWhere(item => item == null || !item.activeInHierarchy || !IsTargetAlive(item));
    }
    protected virtual GameObject GetFirstValidTarget()
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
    protected virtual void CheckAndUpdateTargetCircle()
    {
        CleanUpDestroyedObjects();
        targetToAttack = GetFirstValidTarget();
        if (targetToAttack != null)
        {
            onHaveTarget?.Invoke(targetToAttack);
            targetToAttackPos = targetToAttack.transform.position;
            if (targetCircleInstance != null)
            {
                targetCircleInstance.SetActive(true);
                targetCircleInstance.transform.position = new Vector3(targetToAttackPos.x, targetCircleInstance.transform.position.y, targetToAttackPos.z);
            }
        }
        else
        {
            onHaveTarget?.Invoke(null);
            if (targetCircleInstance != null)
            {
                targetCircleInstance.SetActive(false);
            }
        }
    }

    protected virtual bool IsTargetAlive(GameObject target)
    {
        LifeComponent deadController = target.GetComponent<LifeComponent>();
        return deadController != null && !deadController.IsDead;
    }
    //////////////////Attack theo dir//////////////////
    protected virtual void Attack(Vector3 enemyLoc)
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
            onHaveUlti?.Invoke(false);
        }
    }
    protected virtual void Attack(Vector3 enemyLoc, bool isMainAttack)
    {
        //Overload for child class use cases.
    }
    public void EventIfKillSomeone()
    {
        onKillSomeone?.Invoke();
    }

    public virtual void PrepareToAttack()
    {
        Vector3 attackDir = targetToAttackPos - transform.position;
        onActorAttack?.Invoke(new Vector2(attackDir.x, attackDir.z));
        Attack(targetToAttackPos);
        targetToAttackPos = Vector3.zero;
    }
    protected virtual void InitWeapon(Weapon oldWeapon,Weapon newWeapon)
    {
        ClearOldWeapon();
        weapon = newWeapon;

        weaponOnHand = Instantiate(weapon.WeaponOnHand, WeaponHolder);
        weaponComponent.ApplyWeaponSkin(weaponOnHand, weapon.CurrentIndexOfTheSkin);
        weaponToThrow = Instantiate(weapon.WeaponThrowAway, transform);
        GameObject visualize = Instantiate(weaponOnHand, this.weaponToThrow.transform);


        weaponOnHand.transform.localPosition = weapon.WeaponOffsetPos;
        weaponOnHand.transform.localRotation = weapon.WeaponOffsetRot;
        
        

        weaponToThrow.transform.rotation = Quaternion.Euler(Vector3.zero);
        weaponToThrow.gameObject.SetActive(false);
        
        visualize.transform.localPosition = weapon.WeaponOffsetOnThrow;
        
    }
    protected void ClearOldWeapon()
    {
        if(weaponOnHand!=null)
        Destroy(weaponOnHand.gameObject);
        if(weaponToThrow!=null)
        Destroy(weaponToThrow.gameObject);
    }

    protected void ResetState()
    {
        enemyAttackers.Clear();
        onHaveTarget?.Invoke(null);
        targetToAttack = null;
        targetToAttackPos = Vector3.zero;
    }
    protected void SetUlti(bool isHaveUlti)
    {
        isUlti = isHaveUlti;
    }
    protected virtual void OnUpgrade()
    {
        distanceBuff += CONSTANT_VALUE.CIRCLE_RADIUS_INCREASER;
    }
  
}