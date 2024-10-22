using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ZCAttacker : ActorAttacker
{
    private int moreWeapon = 1;
    [SerializeField] private ZCAttributeController zCAttributeController;
    protected override GameObject GetFirstValidTarget()
    {
        foreach (var target in enemyAttackers)
        {
                return target;   
        }
        return null;
    }
    protected override void CleanUpDestroyedObjects()
    {
        enemyAttackers.RemoveWhere(item => item == null || !item.activeInHierarchy );
    }
    protected override void UpdateEnemyList(GameObject target, bool isInCircle)
    {
        if (isInCircle)
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

    protected override void Attack(Vector3 enemyLoc, bool isMainAttack)
    {
        if (isMainAttack)
        {
            Vector3 throwDirection = enemyLoc - throwLocation.position;
            throwDirection.y = 0;
            throwDirection.Normalize();

            float spreadBetweenProjectiles = 10f;

            float totalSpreadAngle = spreadBetweenProjectiles * (moreWeapon - 1);

            float startingAngle = -totalSpreadAngle / 2f;

            for (int i = 0; i < moreWeapon; i++)
            {
                float currentAngle = startingAngle + (i * spreadBetweenProjectiles);

                Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
                Vector3 adjustedThrowDirection = rotation * throwDirection;

                Vector3 spawnPosition = throwLocation.position + (adjustedThrowDirection * 0.5f);

                Projectile newProjectile = Instantiate(weaponToThrow, spawnPosition, Quaternion.LookRotation(adjustedThrowDirection));
                newProjectile.gameObject.SetActive(true);

                newProjectile.InitForProjectileToThrow(this, weapon.WeaponType,
                    distanceBuff + (actorAtributeController.BuffValues.ContainsKey(Enum.AttributeBuffs.Range)
                        ? actorAtributeController.BuffValues[Enum.AttributeBuffs.Range] : 0)
                );

                newProjectile.FlyToPos(spawnPosition + adjustedThrowDirection * 100f, false);
            }
        }
        else
        {
            Vector3 throwDirection = enemyLoc - throwLocation.position; 
            throwDirection.y = 0; 
            throwDirection.Normalize();

            Vector3 spawnPosition = throwLocation.position + (throwDirection * 0.5f); 

            Projectile newProjectile = Instantiate(weaponToThrow, spawnPosition, Quaternion.LookRotation(throwDirection));
            newProjectile.gameObject.SetActive(true);

            newProjectile.InitForProjectileToThrow(this, weapon.WeaponType,
                distanceBuff + (actorAtributeController.BuffValues.ContainsKey(Enum.AttributeBuffs.Range)
                    ? actorAtributeController.BuffValues[Enum.AttributeBuffs.Range] : 0)
            );

            newProjectile.FlyToPos(spawnPosition + throwDirection * 100f, false);
        }
    }
    //////////////////////////Attack theo gameobjet////////////////////
    protected void Attack(GameObject target)
    {


        for (int i = 0; i < moreWeapon; i++)
        {

            Projectile newProjectile = Instantiate(weaponToThrow, throwLocation.position, Quaternion.identity);
            newProjectile.gameObject.SetActive(true);

            newProjectile.InitForProjectileToThrow(this, weapon.WeaponType,
                distanceBuff + (actorAtributeController.BuffValues.ContainsKey(Enum.AttributeBuffs.Range)
                    ? actorAtributeController.BuffValues[Enum.AttributeBuffs.Range] : 0)
            );

            newProjectile.FlyToPos(target);
        }
    }

    public override void PrepareToAttack()
    {
        ZCPower power = (actorAtributeController as ZCAttributeController).ZCPower1;
        if (power != null)
        {
            switch (power.PowerType) 
            {
                case Enum.ZCPowerUp.CrossAttack:
                    CrossAttack(); 
                    break;

                case Enum.ZCPowerUp.AttackBehind:
                    BehindAttack(); 
                    break;

                case Enum.ZCPowerUp.PursueBullet:
                    ChaseAttack(); 
                    break;

                case Enum.ZCPowerUp.Tripple:
                    TrippleAttack(); 
                    break;

                default:
                    Attack(targetToAttackPos, true);
                    break;
            }
        }
        else
        {
            Attack(targetToAttackPos, true);
            
        }
    }

    private void BehindAttack()
    {
        Vector3 attackDir = targetToAttackPos - throwLocation.position;
        onActorAttack?.Invoke(new Vector2(attackDir.x, attackDir.z));

        Vector3 oppositeAttackDir = -attackDir;

        Vector3 oppositeAttackPos = throwLocation.position + oppositeAttackDir;
        Attack(targetToAttackPos,true);
        Attack(oppositeAttackPos,false);
    }

    private void ChaseAttack()
    {
        onActorAttack?.Invoke(new Vector2(targetToAttack.transform.position.x, targetToAttack.transform.position.z));
        Attack(targetToAttack);
    }
    private void CrossAttack()
    {
        Vector3 attackDir = targetToAttackPos - throwLocation.position; 
        attackDir.y = 0;
        attackDir.Normalize();

        onActorAttack?.Invoke(new Vector2(attackDir.x, attackDir.z));

        Attack(targetToAttackPos,true);

        Vector3 perpendicularDir = Vector3.Cross(attackDir, Vector3.up);

        float sideOffsetDistance = 2f;

        Vector3 leftAttackPos = throwLocation.position + (perpendicularDir * sideOffsetDistance); 
        Vector3 rightAttackPos = throwLocation.position  - (perpendicularDir * sideOffsetDistance); 

        Attack(leftAttackPos, false);
        Attack(rightAttackPos, false); 

        targetToAttackPos = Vector3.zero;
    }
    private void TrippleAttack()
    {
        Vector3 attackDir = targetToAttackPos - throwLocation.position;
        attackDir.y = 0;
        attackDir.Normalize();

        onActorAttack?.Invoke(new Vector2(attackDir.x, attackDir.z));

        Attack(targetToAttackPos, true);

        Vector3 perpendicularDir = Vector3.Cross(attackDir, Vector3.up);

        float sideOffsetDistance = 2f; 
        float angleOffset = 45f; 

        Vector3 leftAttackPos = throwLocation.position +
                                 Quaternion.Euler(0, -angleOffset, 0) * attackDir * sideOffsetDistance;

        Vector3 rightAttackPos = throwLocation.position +
                                  Quaternion.Euler(0, angleOffset, 0) * attackDir * sideOffsetDistance;

        Attack(leftAttackPos, false);
        Attack(rightAttackPos, false);

        targetToAttackPos = Vector3.zero;
    }

    private void SetProjectileNotBeingDestroy()
    {

    }
    IEnumerator ContinousAttack()
    {
        yield return new WaitForSeconds(0.2f);
        Attack(targetToAttackPos, true);
    }
    protected override void OnUpgrade()
    {
        foreach (var stat in (actorAtributeController as ZCAttributeController).Stats)
        {
            if(stat.Type == Enum.ZCUpgradeType.MaxWeapon)
            {
                moreWeapon = stat.HowMuchUpgrade;
            }
        }
    }


}
