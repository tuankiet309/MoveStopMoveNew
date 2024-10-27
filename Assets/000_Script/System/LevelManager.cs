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

    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    public int CurrentPVELevel { get => currentPVELevel; set => currentPVELevel = value; }
    public int CurrentZCLevel { get => currentZCLevel; set => currentZCLevel = value; }
    public List<PVELevel> PVELevels1 { get => PVELevels; set => PVELevels = value; }
    public List<ZCLevel> ZCLevels1 { get => ZCLevels; set => ZCLevels = value; }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        LoadLevel(); 
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
            ZombieSpawner.Instance.NumberOfMaxEnemies = ZCLevels[currentZCLevel].HowManyZombie;
            ZombieSpawner.Instance.BossZombie = ZCLevels[currentZCLevel].BigBoss; 
        }
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

    public int NumberOfEnemyToSpawn { get => numberOfEnemyToSpawn; set => numberOfEnemyToSpawn = value; }
    public GameObject MapToUse { get => mapToUse; set => mapToUse = value; }
    public Transform SpawnPosHolder { get => spawnPosHolder; set => spawnPosHolder = value; }
}
[Serializable]
public class ZCLevel
{
    [SerializeField] private int howManyZombie;
    [SerializeField] private GameObject[] TypeOfEnemies;
    [SerializeField] private Vector3 respawnPosition;
    [SerializeField] private Zombie bigBoss;
    public int HowManyZombie { get => howManyZombie; set => howManyZombie = value; }
    public GameObject[] TypeOfEnemies1 { get => TypeOfEnemies; set => TypeOfEnemies = value; }
    public Vector3 RespawnPosition { get => respawnPosition; set => respawnPosition = value; }
    public Zombie BigBoss { get => bigBoss; set => bigBoss = value; }
}
