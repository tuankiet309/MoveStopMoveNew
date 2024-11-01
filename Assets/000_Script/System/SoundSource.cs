using System.Collections;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;
    private SoundPool objectPool; 
    public AudioSource AudioSource { get => m_AudioSource; set => m_AudioSource = value; }
    public void Initialize(SoundPool pool,Vector3 position)
    {
        objectPool = pool;
        transform.position = position;
    }
    public void PlayAndReturnToPool(AudioClip clip, float volume = 1.0f)
    {
        m_AudioSource.clip = clip;
        m_AudioSource.volume = volume;
        m_AudioSource.Play();
        StartCoroutine(ReturnToPoolAfterPlayback(clip.length));
    }
    private IEnumerator ReturnToPoolAfterPlayback(float duration)
    {
        yield return new WaitForSeconds(duration);
        if (objectPool != null)
        {
            objectPool.Release(this);  
        }
        else
        {
            Debug.LogWarning("Object pool reference is missing!");
        }
    }
}