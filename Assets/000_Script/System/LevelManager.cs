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


    public int CurrentPVELevel { get => currentPVELevel; set => currentPVELevel = value; }
    public int CurrentZCLevel { get => currentZCLevel; set => currentZCLevel = value; }
    public List<PVELevel> PVELevels1 { get => PVELevels; set => PVELevels = value; }
    public List<ZCLevel> ZCLevels1 { get => ZCLevels; set => ZCLevels = value; }

    public void Init()
    {
            GameManager.Instance.gameplayManager.enemySpawner.numberOfMaxEnemies = PVELevels1[currentPVELevel].NumberOfEnemyToSpawn;
            PVELevels[currentPVELevel].MapToUse.SetActive(true);
            GameManager.Instance.gameplayManager.enemySpawner.TransformHolder = PVELevels[currentPVELevel].SpawnPosHolder;
            
            //ZombieSpawner.Instance.NumberOfMaxEnemies = ZCLevels[currentZCLevel].HowManyZombie;
            //ZombieSpawner.Instance.BossZombie = ZCLevels[currentZCLevel].BigBoss;
            //ZombieSpawner.Instance.WhenToSpawnBoss = ZCLevels[currentZCLevel].WhenToSpawnBoss;
            //ZombieSpawner.Instance.GetComponent<ZombiePool>().DogRandom = ZCLevels[currentZCLevel].DogChances;
    }

    public void LoadData(GameData gameData)
    {
        currentPVELevel = gameData.levelData.currentPVELevel;
        currentZCLevel = gameData.levelData.currentZCLevel;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.levelData.currentPVELevel = currentPVELevel;
        gameData.levelData.currentZCLevel = currentZCLevel;
    }


}
[Serializable]
public class PVELevel
{
    [SerializeField] private int numberOfEnemyToSpawn;
    [SerializeField] private Transform spawnPosHolder;
    [SerializeField] private GameObject mapToUse;
    [SerializeField] private bool haveBuff;

    public int NumberOfEnemyToSpawn { get => numberOfEnemyToSpawn; set => numberOfEnemyToSpawn = value; }
    public GameObject MapToUse { get => mapToUse; set => mapToUse = value; }
    public Transform SpawnPosHolder { get => spawnPosHolder; set => spawnPosHolder = value; }
    public bool HaveBuff { get => haveBuff; set => haveBuff = value; }
}
[Serializable]
public class ZCLevel
{
    [SerializeField] private int howManyZombie;
    [SerializeField] private int whenToSpawnBoss;
    [SerializeField] private Vector3 respawnPosition;
    [SerializeField] private Zombie bigBoss;
    [SerializeField] private int dogChances;


    public int HowManyZombie { get => howManyZombie; set => howManyZombie = value; }
    public Vector3 RespawnPosition { get => respawnPosition; set => respawnPosition = value; }
    public Zombie BigBoss { get => bigBoss; set => bigBoss = value; }
    public int WhenToSpawnBoss { get => whenToSpawnBoss; set => whenToSpawnBoss = value; }
    public int DogChances { get => dogChances; set => dogChances = value; }
}
