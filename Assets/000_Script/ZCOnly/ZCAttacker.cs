using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZCAttacker : ActorAttacker
{
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

    protected override void Attack(Vector3 enemyLoc)
    {
        base.Attack(enemyLoc);
    }


}
