using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ParticleBurst : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystems; 
    private ParticlePool particlePool; 
    private bool isBurstComplete = false; 

    private void Start()
    {
    }

    public void InitParticle(ParticlePool pool, Vector3 position, Material mat)
    {
        particleSystems.GetComponent<ParticleSystemRenderer>().material = mat;
        particlePool = pool;
        transform.position = position; 
        EmitParticles(); 
    }

    private void EmitParticles()
    {
        particleSystems.Clear();
        while(particleSystems.particleCount ==0)
            particleSystems.Emit(particleSystems.emission.GetBurst(0).maxCount); 
        StartCoroutine(ReleaseToPoolAfterDelay(1f)); 
    }

    private IEnumerator ReleaseToPoolAfterDelay(float delay)
    {
        while (particleSystems.particleCount > 0) 
        {
            yield return null;
        }
        particlePool.Release(this);
    }
}