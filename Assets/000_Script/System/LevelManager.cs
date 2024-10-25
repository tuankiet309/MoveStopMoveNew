using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] private int currentPVELevel=0;
    [SerializeField] private int currentZCLevel=0;

    [SerializeField] List<PVELevel> PVELevels = new List<PVELevel>();
    [SerializeField] List<ZCLevel> ZCLevels = new List<ZCLevel>();

    private void Start()
    {
        
    }
    private void LoadLevel()
    {
        if(GameManager.Instance.CurrentInGameState == Enum.InGameState.PVE)
        {
            EnemySpawner.Instance.NumberOfMaxEnemies = PVELevels[currentPVELevel].NumberOfEnemyToSpawn;
            PVELevels[currentPVELevel].MapToUse.SetActive(true);
            EnemySpawner.Instance.TransformHolder = PVELevels[currentPVELevel].SpawnPosHolder;
        }
        if(GameManager.Instance.CurrentInGameState == Enum.InGameState.Zombie)
        {

        }
    }









    public void LoadData(GameData gameData)
    {
       
    }

    public void SaveData(ref GameData gameData)
    {
        throw new NotImplementedException();
    }


}
[Serializable]
public class PVELevel
{
    [SerializeField] private int id;
    [SerializeField] private int numberOfEnemyToSpawn;
    [SerializeField] private Transform spawnPosHolder;
    [SerializeField] private GameObject mapToUse;

    public int Id { get => id; set => id = value; }
    public int NumberOfEnemyToSpawn { get => numberOfEnemyToSpawn; set => numberOfEnemyToSpawn = value; }
    public GameObject MapToUse { get => mapToUse; set => mapToUse = value; }
    public Transform SpawnPosHolder { get => spawnPosHolder; set => spawnPosHolder = value; }
}
[Serializable]
public class ZCLevel
{
    [SerializeField] private int id;
    [SerializeField] private int howManyZombie;
    [SerializeField] private GameObject[] TypeOfEnemies;
    [SerializeField] private Vector3 respawnPosition;

    public int Id { get => id; set => id = value; }
    public int HowManyZombie { get => howManyZombie; set => howManyZombie = value; }
    public GameObject[] TypeOfEnemies1 { get => TypeOfEnemies; set => TypeOfEnemies = value; }
    public Vector3 RespawnPosition { get => respawnPosition; set => respawnPosition = value; }
}
