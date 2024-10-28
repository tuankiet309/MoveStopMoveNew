using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiePool : ObjectPoolAbtract<Zombie>
{
    [SerializeField] Zombie zombiePrefab;
    [SerializeField] Zombie dogPrefab;
    int dogRandom = 0;

    public int DogRandom { get => dogRandom; set => dogRandom = value; }

    protected override Zombie CreateObject()
    {
        int randomRange = Random.Range(0, 100);
        if (dogRandom < randomRange)
        {
            Zombie newEnemy = Instantiate(zombiePrefab);
            return newEnemy;
        }
        else
        {
            Zombie newEnemy = Instantiate(dogPrefab);
            return newEnemy;
        }
    }
}


