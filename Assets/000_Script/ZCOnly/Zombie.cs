using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    [SerializeField] private LifeComponent lifeComponent;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    public static int numberOfEnemyHasDie =0;
    public static int numberOfEnemyRightNow=0;

    private void OnEnable()
    {
        numberOfEnemyRightNow++;
    }
    private void OnDisable()
    {
        numberOfEnemyRightNow--;
        numberOfEnemyHasDie++;
    }
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
    public void Init(Material material)
    {
        skinnedMeshRenderer.sharedMaterial = material;
    }
    
    
}
