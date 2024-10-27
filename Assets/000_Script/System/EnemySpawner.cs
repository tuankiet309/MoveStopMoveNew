using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy enemyPreb;
    [SerializeField] private EnemyPool enemyPool;

    [SerializeField] private Skin[] pantToSpawn;
    [SerializeField] private Skin[] bodyToSpawn; 
    [SerializeField] private Skin[] headSetToSpawn;
    [SerializeField] private Skin[] leftHandToSpawn;

    [SerializeField] private Weapon[] weaponToSpawnWith;
    [SerializeField] private string[] nameToSpawnWith;
    [SerializeField] private Transform transformHolder;
    [SerializeField] private int numberOfMaxEnemies = 0;
    [SerializeField][Range(0,12)] protected int numberOfMaxEnemiesAtATime = 0;

    private int numberOfEnemiesSpawned = 0;
    private int numberOfEnemiesLeft;
    private int previousNumberOfEnemiesLeft;
    private bool gameStateChanged = false;
    private Camera mainCamera;

    public  List<Skin> availableBodySkins;

    public UnityEvent<int> OnNumberOfEnemiesDecrease;

    private static EnemySpawner instance;
    public static EnemySpawner Instance { get { return instance; } }

    public int NumberOfEnemiesLeft { get => numberOfEnemiesLeft; private set { } }

    public int NumberOfMaxEnemies { get => numberOfMaxEnemies; set => numberOfMaxEnemies = value; }
    public Transform TransformHolder { get => transformHolder; set => transformHolder = value; }

    private void OnEnable()
    {
        OnNumberOfEnemiesDecrease.Invoke(numberOfEnemiesLeft);
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
        availableBodySkins = new List<Skin>(bodyToSpawn);
        GameManager.Instance.onStateChange.AddListener(SelfActive);
        SelfActive(GameManager.Instance.CurrentGameState, GameManager.Instance.CurrentInGameState);
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
        return numberOfEnemiesSpawned < numberOfMaxEnemies && Enemy.numberOfEnemyRightnow < numberOfMaxEnemiesAtATime && availableBodySkins.Count > 0;
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
        int bodySkinIndex = Random.Range(0, availableBodySkins.Count);

        int randomPant = Random.Range(0, pantToSpawn.Length);
        int randomHeadset = Random.Range(0, headSetToSpawn.Length);
        int randomLeftHand = Random.Range(0, leftHandToSpawn.Length);
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
        enemy.Initialize (availableBodySkins[bodySkinIndex], pantToSpawn[randomPant], headSetToSpawn[randomHeadset], leftHandToSpawn[randomLeftHand], weaponToSpawnWith[randomWeapon], nameToSpawnWith[randomName], enemyPool);
        Skin body = availableBodySkins[bodySkinIndex];
        enemy.onEnemyDie.AddListener(() => ReturnSkinToPool(body));
        availableBodySkins.Remove(availableBodySkins[bodySkinIndex]);
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
        if (gameState == Enum.GameState.Win || gameState == Enum.GameState.Hall)
            gameObject.SetActive(false);
        if (gameState == Enum.GameState.Ingame )
            gameObject.SetActive(true);
    }

    public void ReturnSkinToPool(Skin skin)
    {
        if (!availableBodySkins.Contains(skin))
        {
            availableBodySkins.Add(skin);
        }
    }
}