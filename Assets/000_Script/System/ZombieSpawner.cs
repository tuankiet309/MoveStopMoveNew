using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ZombieSpawner : Spawner<Zombie>
{
    [SerializeField] private Zombie enemyPreb;
    [SerializeField] private ZombiePool enemyPool;
    [SerializeField] private Material[] skinOfZombie;
    [SerializeField] private int numberOfMaxEnemies = 0;
    [SerializeField][Range(0, 50)] private int numberOfMaxEnemiesAtATime = 0;
    [SerializeField] private Transform spawnPosHolder;

    [SerializeField] private Zombie bossZombie;

    private int numberOfEnemiesSpawned = 0;
    private int numberOfEnemiesLeft;
    private int previousNumberOfEnemiesLeft;
    private int whenToSpawnBoss;
    private bool gameStateChanged = false;
    private bool checkBossNotOut = true;
    private Transform player;

    public UnityEvent<int> OnNumberOfEnemiesDecrease;

    private static ZombieSpawner instance;
    public static ZombieSpawner Instance { get { return instance; } }

    public int NumberOfEnemiesLeft { get => numberOfEnemiesLeft; private set { } }
    public int NumberOfMaxEnemies { get => numberOfMaxEnemies; set => numberOfMaxEnemies = value; }
    public int NumberOfMaxEnemiesAtATime { get => numberOfMaxEnemiesAtATime; set => numberOfMaxEnemiesAtATime = value; }
    public Zombie BossZombie { get => bossZombie; set => bossZombie = value; }
    public int WhenToSpawnBoss { get => whenToSpawnBoss; set => whenToSpawnBoss = value; }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        numberOfEnemiesSpawned = 0;
        numberOfEnemiesLeft = numberOfMaxEnemies;
        previousNumberOfEnemiesLeft = numberOfEnemiesLeft;
        Zombie.numberOfEnemyHasDie = 0;
        OnNumberOfEnemiesDecrease.Invoke(numberOfEnemiesLeft);

        GameManager.Instance.onStateChange.AddListener(SelfActive);

        SelfActive(GameManager.Instance.CurrentGameState, GameManager.Instance.CurrentInGameState);
    }

    private void Update()
    {
        UpdateEnemyCount();
    }

    private void OnEnable()
    {
        OnNumberOfEnemiesDecrease.Invoke(numberOfEnemiesLeft);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    protected override bool CanSpawn()
    {
        return numberOfEnemiesSpawned < numberOfMaxEnemies && Zombie.numberOfEnemyRightNow < numberOfMaxEnemiesAtATime && numberOfEnemiesLeft > 0;
    }

    IEnumerator SpawnZombieContinuously()
    {
        while (numberOfEnemiesLeft > 0)
        {
            if (CanSpawn())
                SpawnEntity();
            yield return new WaitForSeconds(0.2f); 
        }
    }

    protected override void SpawnEntity()
    {
        if (numberOfEnemiesLeft < whenToSpawnBoss && checkBossNotOut && bossZombie != null)
        {
            Zombie bossInstance = Instantiate(bossZombie, FindClosestSpawnPosition().position, Quaternion.identity);
            numberOfMaxEnemies++;
            numberOfEnemiesSpawned++;
            numberOfEnemiesLeft++;  
            checkBossNotOut = false; 
            Debug.Log("Boss spawned! Increasing enemies left by 1.");
            OnNumberOfEnemiesDecrease?.Invoke(numberOfEnemiesLeft);
            return;  
        }

        Zombie newEnemy = enemyPool.Get();
        InitializeEntity(newEnemy);
        numberOfEnemiesSpawned++;
        numberOfEnemiesLeft = numberOfMaxEnemies - Zombie.numberOfEnemyHasDie;

        OnNumberOfEnemiesDecrease?.Invoke(numberOfEnemiesLeft);
    }

    protected override void InitializeEntity(Zombie enemy)
    {
        int random = Random.Range(0, skinOfZombie.Length);
        enemy.Init(skinOfZombie[random]);

        Transform spawnPoint = FindClosestSpawnPosition();

        if (spawnPoint != null)
        {
            Vector3 randomOffset = GetRandomOffset(3f);
            enemy.transform.position = spawnPoint.position + randomOffset;
        }
        else
        {
            Debug.LogWarning("No valid spawn position found!");
        }
    }

    private Transform FindClosestSpawnPosition()
    {
        List<Transform> validSpawnPoints = new List<Transform>();

        foreach (Transform spawnPoint in spawnPosHolder)
        {
            float distanceToPlayer = Vector3.Distance(player.position, spawnPoint.position);

            if (distanceToPlayer >= 20f)
                validSpawnPoints.Add(spawnPoint);
        }

        var closestSpawnPoints = validSpawnPoints
            .OrderBy(spawnPoint => Vector3.Distance(player.position, spawnPoint.position))
            .Take(5)
            .ToList();

        if (closestSpawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, closestSpawnPoints.Count);
            return closestSpawnPoints[randomIndex];
        }

        return null;  
    }

    private Vector3 GetRandomOffset(float range)
    {
        float randomX = Random.Range(-range, range);
        float randomZ = Random.Range(-range, range);
        return new Vector3(randomX, 0, randomZ);
    }

    private void UpdateEnemyCount()
    {
        numberOfEnemiesLeft = numberOfMaxEnemies - Zombie.numberOfEnemyHasDie;
        if (numberOfEnemiesLeft != previousNumberOfEnemiesLeft)
        {
            OnNumberOfEnemiesDecrease.Invoke(numberOfEnemiesLeft);
            previousNumberOfEnemiesLeft = numberOfEnemiesLeft;
        }

        if (numberOfEnemiesLeft <= 0 && !gameStateChanged && GameManager.Instance.CurrentGameState != Enum.GameState.Dead)
        {
            gameStateChanged = true;
            GameManager.Instance.SetGameStates(Enum.GameState.Win, Enum.InGameState.Zombie);
        }
    }

    private void SelfActive(Enum.GameState gameState, Enum.InGameState inGameState)
    {
        if (gameState == Enum.GameState.Win || gameState == Enum.GameState.Ingame)
        {
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
        else if (gameState == Enum.GameState.Begin)
        {
            gameObject.SetActive(true);
            StartCoroutine(SpawnZombieContinuously());
        }
    }

    protected override Zombie GetFromPool()
    {
        return null;
    }
}