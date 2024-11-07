using System.Collections;
using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    [SerializeField] private BuffPool buffPool; 
    [SerializeField] private Transform spawnLocations; 
    [SerializeField] private int maxBuffsAtATime = 5; 
    [SerializeField] private float spawnCooldown = 5f; 

    private int currentBuffCount = 0; 
    private bool isSpawning = false;


    public void InitBuffSpawner()
    {
        if (GameManager.Instance.levelManager.PVELevels1[GameManager.Instance.levelManager.CurrentPVELevel].HaveBuff)
        {
            StartCoroutine(SpawnBuffs());

        }
    }
    private IEnumerator SpawnBuffs()
    {
        while (true)
        {
            if (!isSpawning && currentBuffCount <= maxBuffsAtATime)
            {
                SpawnBuff();
            }
            yield return new WaitForSeconds(spawnCooldown); 
        }
    }

    private void SpawnBuff()
    {
        isSpawning = true;

        int randomIndex = Random.Range(0, spawnLocations.childCount);
        Vector3 spawnPosition = spawnLocations.GetChild(randomIndex).position;
        spawnPosition = new Vector3(spawnPosition.x,10,spawnPosition.z);
        BuffGift newBuff = buffPool.Get();
        newBuff.transform.position = spawnPosition;

        currentBuffCount++;
        newBuff.onBuffPickup.AddListener(() => ReturnBuffToPool(newBuff));
        isSpawning = false;
    }

    private void ReturnBuffToPool(BuffGift buff)
    {
        buffPool.Release(buff); 
        currentBuffCount--; 
    }
}