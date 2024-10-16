using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : ObjectPoolAbtract<Enemy>
{
    [SerializeField] Enemy enemyToPool;

    protected override Enemy CreateObject()
    {
        Enemy newEnemy = Instantiate(enemyToPool);
        return newEnemy;
    }
}
