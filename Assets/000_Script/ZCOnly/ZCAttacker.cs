using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class ZCAttacker : ActorAttacker
{
    private int moreWeapon = 1;
    private ZCAttributeController zCAttributeController;

    private bool isGrowing = false;

    protected override void Start()
    {
        base.Start();
        zCAttributeController = actorAtributeController as ZCAttributeController;
        zCAttributeController.onPlayerUpgraded.AddListener(OnPlayerUpgrade);
    }
    protected override GameObject GetFirstValidTarget()
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

        return closestTarget; // Returns the closest enemy or null if there are no valid targets.
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

            float spreadBetweenProjectiles = 20f;

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

                newProjectile.FlyToPos(spawnPosition + adjustedThrowDirection * 100f, isGrowing);
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

            newProjectile.FlyToPos(spawnPosition + throwDirection * 100f, isGrowing);
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
                    Attack(targetToAttackPos, true);
                    break;
                case Enum.ZCPowerUp.BulletPlus:
                    moreWeapon++;
                    Attack(targetToAttackPos, true);
                    onActorAttack?.Invoke(targetToAttackPos);

                    break;
                case Enum.ZCPowerUp.Continous:
                    Attack(targetToAttackPos, true);
                    StartCoroutine(ContinousAttack());
                    onActorAttack?.Invoke(targetToAttackPos);
                    break;
                default:
                    Attack(targetToAttackPos, true);
                    onActorAttack?.Invoke(targetToAttackPos);
                    break;
            }
        }
        else
        {
            Attack(targetToAttackPos, true);
            onActorAttack?.Invoke(new Vector2(targetToAttack.transform.position.x, targetToAttack.transform.position.z));

        }
    }

    private void SetPiercingThroughEnemy()
    {
        weaponToThrow.GetComponent<DamageComponent>().IsDestroyedAfterCollide = false;
        weaponToThrow.IsGoThroughWall = true;
        Attack(targetToAttackPos, true);
    }
    private void SetThroughWall()
    {
        weaponToThrow.IsGoThroughWall = true;
        Attack(targetToAttackPos, true);
    }
    private void BehindAttack()
    {
        Vector3 attackDir = targetToAttackPos - throwLocation.position;

        Vector3 oppositeAttackDir = -attackDir;

        Vector3 oppositeAttackPos = throwLocation.position + oppositeAttackDir;
        Attack(targetToAttackPos,true);
        Attack(oppositeAttackPos,false);
        onActorAttack?.Invoke(targetToAttackPos);

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


        Attack(targetToAttackPos,true);

        Vector3 perpendicularDir = Vector3.Cross(attackDir, Vector3.up);

        float sideOffsetDistance = 2f;

        Vector3 leftAttackPos = throwLocation.position + (perpendicularDir * sideOffsetDistance); 
        Vector3 rightAttackPos = throwLocation.position  - (perpendicularDir * sideOffsetDistance); 

        Attack(leftAttackPos, false);
        Attack(rightAttackPos, false);
        onActorAttack?.Invoke(targetToAttackPos);
        targetToAttackPos = Vector3.zero;
    }
    private void TrippleAttack()
    {
        Vector3 attackDir = targetToAttackPos - throwLocation.position;
        attackDir.y = 0;
        attackDir.Normalize();


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
        onActorAttack?.Invoke(targetToAttackPos);

        targetToAttackPos = Vector3.zero;
    }

    IEnumerator ContinousAttack()
    {
        yield return new WaitForSeconds(0.2f);
        Attack(targetToAttackPos, true);
    }
    protected virtual void OnPlayerUpgrade()
    {
        moreWeapon++;
    }


}
