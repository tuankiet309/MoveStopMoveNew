using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticlePool particlePool;
    private static ParticleManager instance;
    public static ParticleManager Instance {  get { return instance; } }
    public void InitParticleManager()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void PlayParticle(Vector3 position, Material mat)
    {
        ParticleBurst burst = particlePool.Get();
        burst.InitParticle(particlePool, position, mat);
    }
}
