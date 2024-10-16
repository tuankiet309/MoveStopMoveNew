using System.Collections;
using UnityEngine;

public class ParticleBurst : MonoBehaviour
{
    [SerializeField] ParticleSystem particleSystems;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] LifeComponent lifeComponents;

    private void Start()
    {
        lifeComponents.onLifeEnds.AddListener(BurstParticle);
        
    }

    private void OnEnable()
    {
        lifeComponents.onLifeEnds.AddListener(BurstParticle);
        
    }

    private void BurstParticle(string tempo)
    {
        StartCoroutine(BurstCoroutine());
    }

    private IEnumerator BurstCoroutine()
    {
        ParticleSystemRenderer particleSystemRenderer = particleSystems.GetComponent<ParticleSystemRenderer>();
        particleSystemRenderer.material = skinnedMeshRenderer.material;
        do
        {
            particleSystems.Emit(particleSystems.emission.GetBurst(0).maxCount);
            yield return null; 
        }
        while (particleSystems.particleCount == 0);
    }
}