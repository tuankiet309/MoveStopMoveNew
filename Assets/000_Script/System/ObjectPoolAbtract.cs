using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class ObjectPoolAbtract<T> : MonoBehaviour where T : MonoBehaviour
{
    protected IObjectPool<T> objectPool; 

    protected virtual void Awake()
    {
        objectPool = new ObjectPool<T>(
            CreateObject,      
            OnTakeFromPool,    
            OnReturnedToPool,   
            OnDestroyPoolObject,
            false,
            20,
            100
        );
    }

    protected abstract T CreateObject();

    protected virtual void OnTakeFromPool(T obj)
    {
        obj.gameObject.SetActive(true);
    }


    protected virtual void OnReturnedToPool(T obj)
    {
        obj.gameObject.SetActive(false); 
    }

    protected virtual void OnDestroyPoolObject(T obj)
    {
        Destroy(obj.gameObject); 
    }

    public T Get()
    {
        return objectPool.Get();
    }

    public void Release(T obj)
    {
        objectPool.Release(obj);
    }
}