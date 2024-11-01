using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePool : ObjectPoolAbtract<ParticleBurst>
{
    [SerializeField] private ParticleBurst burstPref;

    protected override ParticleBurst CreateObject()
    {
        ParticleBurst particleBurst= Instantiate(burstPref,gameObject.transform);
        return particleBurst;
    }
}
