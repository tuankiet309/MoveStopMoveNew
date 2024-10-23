using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class ZombieSpawner : Spawner<Zombie>
{
    [SerializeField] private Zombie enemyPreb;
    [SerializeField] private ZombiePool enemyPool;
    [SerializeField] Material[] skinOfZombie;

    [SerializeField] protected int numberOfMaxEnemies = 0;
    [SerializeField][Range(0, 30)] protected int numberOfMaxEnemiesAtATime = 0;
    [SerializeField] protected Transform spawnPosHolder;

    private int numberOfEnemiesSpawned = 0;
    private int numberOfEnemiesLeft;
    private int previousNumberOfEnemiesLeft;
    private bool gameStateChanged = false;
    private Camera mainCamera;

    public UnityEvent<int> OnNumberOfEnemiesDecrease;

    private static ZombieSpawner instance;
    public static ZombieSpawner Instance { get { return instance; } }

    public int NumberOfEnemiesLeft { get => numberOfEnemiesLeft; private set { } }

    private void OnDestroy()
    {
        GameManager.Instance.onStateChange.RemoveListener(SelfActive);
    }

    private void OnEnable()
    {
        OnNumberOfEnemiesDecrease.Invoke(numberOfEnemiesLeft);
        StartCoroutine(SpawnZombieSlowly());

    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private void Awake()
    {
        mainCamera = Camera.main;
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
        SelfActive(Enum.GameState.Ingame, Enum.InGameState.Zombie);
    }

    private void Update()
    {
        //if (CanSpawn())
        //{
        //    StartCoroutine(SpawnZombieSlowly());
        //}
        UpdateEnemyCount();
    }

    protected override bool CanSpawn()
    {
        return numberOfEnemiesSpawned < numberOfMaxEnemies && Zombie.numberOfEnemyRightNow < numberOfMaxEnemiesAtATime;
    }

    IEnumerator SpawnZombieSlowly()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if(CanSpawn())
                SpawnEntity();
        }
    }
    protected override void SpawnEntity()
    {
        Zombie newEnemy = enemyPool.Get();
        InitializeEntity(newEnemy);
        numberOfEnemiesSpawned++;
        numberOfEnemiesLeft = numberOfMaxEnemies - Enemy.numberOfEnemyHasDie;
        OnNumberOfEnemiesDecrease?.Invoke(numberOfEnemiesLeft);
    }

    protected override void InitializeEntity(Zombie enemy)
    {
        int random = Random.Range(0, skinOfZombie.Length);
        enemy.Init(skinOfZombie[random]);

        Transform nearestOffscreenSpawn = FindNearestOffscreenSpawnPosition();

        if (nearestOffscreenSpawn != null)
        {
            Vector3 randomOffset = GetRandomOffset(3f); 
            enemy.transform.position = nearestOffscreenSpawn.position + randomOffset;
        }
        else
        {
            Debug.LogWarning("No valid offscreen spawn position found!");
        }
    }

    private Transform FindNearestOffscreenSpawnPosition()
    {
        Transform nearestSpawnPoint = null;
        float nearestDistance = float.MaxValue;
        float minimumDistance = 10f;

        foreach (Transform spawnPoint in spawnPosHolder)
        {
            if (!IsOnScreen(spawnPoint.position))
            {
                float distanceToCamera = Vector3.Distance(mainCamera.transform.position, spawnPoint.position);

                if (distanceToCamera < nearestDistance && distanceToCamera > minimumDistance)
                {
                    nearestDistance = distanceToCamera;
                    nearestSpawnPoint = spawnPoint;
                }
            }
        }

        return nearestSpawnPoint;
    }

    private Vector3 GetRandomOffset(float range)
    {
        float randomX = Random.Range(-range, range);
        float randomZ = Random.Range(-range, range);
        return new Vector3(randomX, 0, randomZ); 
    }



    private bool IsOnScreen(Vector3 position)
    {
        Vector3 screenPoint = mainCamera.WorldToViewportPoint(position);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }

    private void UpdateEnemyCount()
    {
        numberOfEnemiesLeft = numberOfMaxEnemies - Enemy.numberOfEnemyHasDie;
        if (numberOfEnemiesLeft != previousNumberOfEnemiesLeft)
        {
            OnNumberOfEnemiesDecrease.Invoke(numberOfEnemiesLeft);
            previousNumberOfEnemiesLeft = numberOfEnemiesLeft;
        }

        if (numberOfEnemiesLeft <= 0 && !gameStateChanged && GameManager.Instance.CurrentGameState != Enum.GameState.Dead)
        {
            gameStateChanged = true;
            GameManager.Instance.SetGameStates(Enum.GameState.Win, Enum.InGameState.PVE);
        }
    }

    private void SelfActive(Enum.GameState gameState, Enum.InGameState inGameState)
    {
        if (gameState == Enum.GameState.Win || gameState == Enum.GameState.Ingame)
            gameObject.SetActive(false);
        if (gameState == Enum.GameState.Begin)
            gameObject.SetActive(true);
    }

    protected override Zombie GetFromPool()
    {
        return null;
    }
}