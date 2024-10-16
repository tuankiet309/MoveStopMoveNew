using UnityEngine;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{
    protected abstract T GetFromPool(); 
    protected abstract void InitializeEntity(T entity); 

    protected abstract bool CanSpawn(); 
    protected abstract void SpawnEntity(); 
}