using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPreb; 
    [SerializeField] private EnemyPool enemyPool;
    [SerializeField] private Material[] skinColorToSpawn;
    [SerializeField] private Material[] pantColorToSpawn;
    [SerializeField] private Weapon[] weaponToSpawnWith;
    [SerializeField] private string[] nameToSpawnWith;
    [SerializeField] private Transform transformHolder;
    [SerializeField] protected int numberOfMaxEnemies = 0;
    [SerializeField] protected int numberOfMaxEnemiesAtATime = 0;

    private int numberOfEnemiesSpawned = 0;
    private int numberOfEnemiesLeft;
    private int previousNumberOfEnemiesLeft;
    private bool gameStateChanged = false;
    private Camera mainCamera;

    private List<Material> availableSkin;


    public UnityEvent<int> OnNumberOfEnemiesDecrease;

    private static EnemySpawner instance;
    public static EnemySpawner Instance { get { return instance; } }

    public int NumberOfEnemiesLeft { get => numberOfEnemiesLeft; private set { } }

    private void OnDestroy()
    {
        GameManager.Instance.onStateChange.RemoveListener(SelfActive);
    }

    private void OnEnable()
    {
        GameManager.Instance.onStateChange.AddListener(SelfActive);
        OnNumberOfEnemiesDecrease.Invoke(numberOfEnemiesLeft);
        availableSkin = new List<Material>(skinColorToSpawn);
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
        Enemy.numberOfEnemyHasDie = 0;
        OnNumberOfEnemiesDecrease.Invoke(numberOfEnemiesLeft);
        availableSkin = new List<Material>(skinColorToSpawn);
    }

    private void Update()
    {
        if (CanSpawn())
        {
            SpawnEntity();
        }

        UpdateEnemyCount();
    }

    protected virtual bool CanSpawn()
    {
        return numberOfEnemiesSpawned < numberOfMaxEnemies && Enemy.numberOfEnemyRightnow < numberOfMaxEnemiesAtATime;
    }

    protected virtual void SpawnEntity()
    {
        Enemy newEnemy = enemyPool.Get();
        InitializeEntity(newEnemy);
        numberOfEnemiesSpawned++;
        numberOfEnemiesLeft = numberOfMaxEnemies - Enemy.numberOfEnemyHasDie;
        OnNumberOfEnemiesDecrease?.Invoke(numberOfEnemiesLeft);
    }

    protected virtual void InitializeEntity(Enemy enemy)
    {
        int randomSkin = Random.Range(0,availableSkin.Count);
        int randomPant = Random.Range(0, pantColorToSpawn.Length);
        int randomWeapon = Random.Range(0, weaponToSpawnWith.Length);
        int randomName = Random.Range(0, nameToSpawnWith.Length);
        Vector3 pos;
        do
        {
            int randomPos = Random.Range(0, transformHolder.childCount);
            pos = transformHolder.GetChild(randomPos).position;
            pos = new Vector3(pos.x, enemy.transform.position.y, pos.z);
        } while (IsOnScreen(pos));
        enemy.transform.position = pos;
        enemy.Initialize(skinColorToSpawn[randomSkin], pantColorToSpawn[randomPant], weaponToSpawnWith[randomWeapon],nameToSpawnWith[randomName],enemyPool);
        Debug.Log("Removing skin: " + availableSkin[randomSkin].name);
        availableSkin.Remove(availableSkin[randomSkin]);
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
            GameManager.Instance.SetGameState(Enum.GameState.Win);
        }
    }
    private void SelfActive(Enum.GameState gameState)
    {
        if (gameState == Enum.GameState.Win || gameState == Enum.GameState.Hall)
            gameObject.SetActive(false);
        if (gameState == Enum.GameState.Zone1 || gameState == Enum.GameState.Zone2)
            gameObject.SetActive(true);
    }

    public void ReturnSkinToPool(Material skin)
    {
        if (!availableSkin.Contains(skin))
        {
            availableSkin.Add(skin);
            Debug.Log("Skin returned to pool: " + skin.name);
        }
    }
}