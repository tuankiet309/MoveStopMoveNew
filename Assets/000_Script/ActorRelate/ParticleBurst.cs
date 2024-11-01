using System.Collections;
using UnityEngine;

public class ParticleBurst : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystems;
    private ParticlePool particlePool;

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

        particleSystems.Emit(20);


        StartCoroutine(ReleaseToPoolAfterDelay());
    }

    private IEnumerator ReleaseToPoolAfterDelay()
    {
        while(particleSystems.particleCount > 0)
            yield return null;
        particlePool.Release(this);
    }
}