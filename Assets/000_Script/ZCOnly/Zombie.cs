using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private LifeComponent lifeComponent;
    void Start()
    {
        lifeComponent.onLifeEnds.AddListener(SelfDestroy);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void SelfDestroy(string tempo)
    {

        gameObject.SetActive(false);
    }
    
    
}
