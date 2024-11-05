using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZCBossLifeComponent : LifeComponent
{
    
    public  void Start()
    {
        health = 15;    
    }

    public override bool DamageHealth(string attackerName)
    {
        health -= 1;
        transform.localScale -= Vector3.one * 0.1f;
        if (health <= 0)
        {
            ParticleSpawner.Instance.PlayParticle(transform.position + Vector3.up,actorMeshRenderer.sharedMaterial);
            onLifeEnds?.Invoke(attackerName);
            return true;
        }
        else
            return false;
    }
}
