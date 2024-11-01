using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    [SerializeField] private ParticlePool particlePool;
    private static ParticleSpawner instance;
    public static ParticleSpawner Instance {  get { return instance; } }
    private void Awake()
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
