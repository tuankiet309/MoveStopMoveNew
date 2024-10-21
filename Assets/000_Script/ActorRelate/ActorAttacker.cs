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

    protected HashSet<GameObject> enemyAttackers = new HashSet<GameObject>();
    protected GameObject targetToAttack = null;
    protected Vector3 targetToAttackPos = Vector3.zero;
    protected Weapon weapon;
    protected Projectile weaponToThrow;
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
        ResetState();
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
    protected virtual void Attack(Vector3 enemyLoc)
    {
        Vector3 throwDirection = enemyLoc - throwLocation.position;
        throwDirection.y = 0;
        Quaternion throwRotation = Quaternion.LookRotation(throwDirection);
        Projectile newProjectile = Instantiate(weaponToThrow, throwLocation.position, throwRotation);
        newProjectile.gameObject.SetActive(true);
        newProjectile.InitForProjectileToThrow(this, weapon.WeaponType, actorAtributeController.BuffValues.ContainsKey(Enum.AttributeBuffs.Range)  ? actorAtributeController.BuffValues[Enum.AttributeBuffs.Range] : 0);
        newProjectile.FlyToPos(enemyLoc, isUlti);
        if (isUlti)
        {
            onHaveUlti?.Invoke(false);
        }
    }
    public void EventIfKillSomeone()
    {
        onKillSomeone?.Invoke();
    }
    public void PrepareToAttack()
    {
        Vector3 attackDir = targetToAttackPos - transform.position;
        onActorAttack?.Invoke(new Vector2(attackDir.x, attackDir.z));
        Attack(targetToAttackPos);
        targetToAttackPos = Vector3.zero;
    }
    private void InitWeapon(Weapon oldWeapon,Weapon newWeapon)
    {
        this.weapon = newWeapon;
        this.weaponToThrow = Instantiate(weapon.WeaponThrowAway, transform);
        GameObject visualize = Instantiate(weapon.WeaponOnHand, this.weaponToThrow.transform);
        this.weaponToThrow.gameObject.SetActive(false);

        
    }
    private void ResetState()
    {
        enemyAttackers.Clear();
        onHaveTarget?.Invoke(null);
        targetToAttack = null;
        targetToAttackPos = Vector3.zero;
    }
    private void SetUlti(bool isHaveUlti)
    {
        isUlti = isHaveUlti;
    }
  
}