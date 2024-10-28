using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZCBossLifeComponent : LifeComponent
{
    
    protected override void Start()
    {
        base.Start();
        health = 10;    
    }

    public override bool DamageHealth(string attackerName)
    {
        health -= 1;
        transform.localScale -= Vector3.one * 0.1f;
        if (health <= 0)
        {
            onLifeEnds?.Invoke(attackerName);
            return true;
        }
        else
            return false;
    }
}
