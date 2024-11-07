using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    private static GameplayManager instance;
    public static GameplayManager Instance {  get { return instance; } set { instance = value; } }

    [Header("______________________Gameplay Essential__________________")]
    public EnemySpawner enemySpawner;
    public BuffSpawner buffSpawner;
    public Player player;
    public List<Enemy> enemies;
    public GameplayUI gameplayUI;
    [Header("_____________________UI Pref_______________________________")]
    public GameplayUI gameplayUIPref;

    public void Init()
    {
        GameManager.Instance.gameplayManager = instance;
        enemySpawner.Init();
        player.Init();
        buffSpawner.InitBuffSpawner();
    }
    public void InitGameplay()
    {
        gameplayUI = Instantiate(gameplayUI);
    }
}
