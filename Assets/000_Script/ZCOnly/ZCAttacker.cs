using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ZCAttacker : ActorAttacker
{
    private int moreWeapon = 1;
    private ZCAttributeController zCAttributeController;

    private bool isGrowing = false;

    public  void Start()
    {
        
        zCAttributeController = actorAtributeController as ZCAttributeController;
    }
    public override GameObject GetFirstValidTarget()
    {
        GameObject closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (var target in enemyAttackers)
        {
            if (target == null) continue; 

            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = target;
            }
        }

        return closestTarget; 
    }
    public override void CleanUpDestroyedObjects()
    {
        enemyAttackers.RemoveWhere(item => item == null || !item.activeInHierarchy );
    }
    public override void UpdateEnemyList(GameObject target, bool isInCircle)
    {
        if (isInCircle)
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
    public override void Attack(Vector3 enemyLoc,Vector3 throwLocationTempo, bool isMainAttack)
    {
        if (isMainAttack)
        {
            Vector3 throwDirection = enemyLoc - throwLocationTempo;
            throwDirection.y = 0;
            throwDirection.Normalize();

            float spreadBetweenProjectiles = 20f;

            float totalSpreadAngle = spreadBetweenProjectiles * (moreWeapon - 1);

            float startingAngle = -totalSpreadAngle / 2f;

            for (int i = 0; i < moreWeapon; i++)
            {
                float currentAngle = startingAngle + (i * spreadBetweenProjectiles);

                Quaternion rotation = Quaternion.AngleAxis(currentAngle, Vector3.up);
                Vector3 adjustedThrowDirection = rotation * throwDirection;

                Vector3 spawnPosition = throwLocationTempo + (adjustedThrowDirection * 0.5f);

                Projectile newProjectile = Instantiate(weaponToThrow, spawnPosition, Quaternion.LookRotation(adjustedThrowDirection));
                newProjectile.gameObject.SetActive(true);

                newProjectile.InitForProjectileToThrow(this, weapon.WeaponType,
                    distanceBuff + (actorAtributeController.BuffValues.ContainsKey(Enum.AttributeBuffs.Range)
                        ? actorAtributeController.BuffValues[Enum.AttributeBuffs.Range] : 0)
                );

                newProjectile.FlyToPos(spawnPosition + adjustedThrowDirection * 100f, isGrowing);
            }
        }
        else
        {
            Vector3 throwDirection = enemyLoc - throwLocationTempo; 
            throwDirection.y = 0; 
            throwDirection.Normalize();

            Vector3 spawnPosition = throwLocationTempo + (throwDirection * 0.5f); 

            Projectile newProjectile = Instantiate(weaponToThrow, spawnPosition, Quaternion.LookRotation(throwDirection));
            newProjectile.gameObject.SetActive(true);

            newProjectile.InitForProjectileToThrow(this, weapon.WeaponType,
                distanceBuff + (actorAtributeController.BuffValues.ContainsKey(Enum.AttributeBuffs.Range)
                    ? actorAtributeController.BuffValues[Enum.AttributeBuffs.Range] : 0)
            );

            newProjectile.FlyToPos(spawnPosition + throwDirection * 100f, isGrowing);
        }
    }
    //////////////////////////Attack theo gameobjet////////////////////
    public void Attack(GameObject target)
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
    private bool isMoreWeapon = true;
    public override void PrepareToAttack()
    {
        ZCPower power = zCAttributeController.ZCPower1;
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
                case Enum.ZCPowerUp.IgnoreWall:
                    SetThroughWall();
                    break;
                case Enum.ZCPowerUp.PieceWeapon:
                    SetPiercingThroughEnemy();
                    break;
                case Enum.ZCPowerUp.GrowWeapon:
                    isGrowing = true;
                    Attack(targetToAttackPos,throwLocation.position, true);
                    break;
                case Enum.ZCPowerUp.BulletPlus:
                    if (isMoreWeapon)
                    {
                        moreWeapon++;
                        isMoreWeapon = false;
                    }
                    Attack(targetToAttackPos, throwLocation.position, true);
                    CallEventRelateToAttack();

                    break;
                case Enum.ZCPowerUp.Continous:
                    Attack(targetToAttackPos, throwLocation.position, true);
                    Vector3 temp = targetToAttackPos;
                    Vector3 temp2 = throwLocation.position;
                    CallEventRelateToAttack();
                    StartCoroutine(ContinousAttack(temp,temp2));
                    break;
                default:
                    Attack(targetToAttackPos, throwLocation.position,true);
                    CallEventRelateToAttack();
                    break;
            }
        }
        else
        {
            CallEventRelateToAttack();
            Attack(targetToAttackPos, throwLocation.position, true);
        }
    }
    private void SetPiercingThroughEnemy()
    {
        weaponToThrow.GetComponent<DamageComponent>().IsDestroyedAfterCollide = false;
        weaponToThrow.IsGoThroughWall = true;
        Attack(targetToAttackPos, throwLocation.position, true);
    }
    private void SetThroughWall()
    {
        weaponToThrow.IsGoThroughWall = true;
        Attack(targetToAttackPos, throwLocation.position, true);
    }
    private void BehindAttack()
    {
        Vector3 attackDir = targetToAttackPos - throwLocation.position;
        Vector3 oppositeAttackDir = -attackDir;
        Vector3 oppositeLocalPosition = -throwLocation.localPosition;
        Vector3 oppositeAttackPos = throwLocation.parent.TransformPoint(oppositeLocalPosition);
        Attack(targetToAttackPos, throwLocation.position, true);
        Attack(oppositeAttackPos, throwLocation.position, false);

        CallEventRelateToAttack();
    }
    private void ChaseAttack()
    {
        CallEventRelateToAttack();
        Attack(targetToAttack);
    }
    private void CrossAttack()
    {
        Vector3 attackDir = targetToAttackPos - throwLocation.position; 
        attackDir.y = 0;
        attackDir.Normalize();


        Attack(targetToAttackPos, throwLocation.position,true);

        Vector3 perpendicularDir = Vector3.Cross(attackDir, Vector3.up);

        float sideOffsetDistance = 2f;

        Vector3 leftAttackPos = transform.position + (perpendicularDir * sideOffsetDistance); 
        Vector3 rightAttackPos = transform.position  - (perpendicularDir * sideOffsetDistance); 

        Attack(leftAttackPos, transform.position, false);
        Attack(rightAttackPos, transform.position, false);
        CallEventRelateToAttack();
        targetToAttackPos = Vector3.zero;
    }
    private void TrippleAttack()
    {
        Vector3 attackDir = targetToAttackPos - throwLocation.position;
        attackDir.y = 0;
        attackDir.Normalize();


        Attack(targetToAttackPos, throwLocation.position, true);

        Vector3 perpendicularDir = Vector3.Cross(attackDir, Vector3.up);

        float sideOffsetDistance = 2f; 
        float angleOffset = 45f; 

        Vector3 leftAttackPos = throwLocation.position +
                                 Quaternion.Euler(0, -angleOffset, 0) * attackDir * sideOffsetDistance;

        Vector3 rightAttackPos = throwLocation.position +
                                  Quaternion.Euler(0, angleOffset, 0) * attackDir * sideOffsetDistance;

        Attack(leftAttackPos, throwLocation.position, false);
        Attack(rightAttackPos, throwLocation.position, false);
        CallEventRelateToAttack();

        targetToAttackPos = Vector3.zero;
    }
    IEnumerator ContinousAttack(Vector3 temp,Vector3 throwLoc)
    {
        yield return new WaitForSeconds(0.2f);
        Attack(temp, throwLoc, true);
    }
    public virtual void OnPlayerUpgrade()
    {
        moreWeapon++;
    }
}
