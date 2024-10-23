using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : ObjectPoolAbtract<Zombie>
{
    [SerializeField] Zombie zombiePrefab;
    protected override Zombie CreateObject()
    {
        Zombie newEnemy = Instantiate(zombiePrefab);
        return newEnemy;
    }

    
}
