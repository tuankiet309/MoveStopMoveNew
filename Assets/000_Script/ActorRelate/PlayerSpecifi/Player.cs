using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static Player instance;
    public static Player Instance { get { return instance; } private set { } }

    [SerializeField] private Canvas[] CanvasNeedToTurnOnOff;
    [SerializeField] private Transform[] transformHolder;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
        

    }
    private void Start()
    {
        GameManager.Instance.onStateChange.AddListener(PrepareGamestate);
        PrepareGamestate(GameManager.Instance.CurrentGameState, GameManager.Instance.CurrentInGameState);
    }
    private void PrepareGamestate(Enum.GameState gameState, Enum.InGameState inGameState)
    {
        
        if (gameState == Enum.GameState.Hall )
        {
            transform.position = new Vector3(0, transform.position.y, 0);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            TurnWorldCanvas(false);
        }
        if ((gameState == Enum.GameState.Ingame && inGameState == Enum.InGameState.PVE)|| (gameState == Enum.GameState.Begin && inGameState == Enum.InGameState.Zombie))
        {
            if(inGameState == Enum.InGameState.PVE)
                transform.position = new Vector3(0, transform.position.y, 0);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            TurnWorldCanvas(true);
        }
        if (gameState == Enum.GameState.Dead)
        {
            TurnWorldCanvas(false);
        }
        if(gameState == Enum.GameState.Win)
        {
            TurnWorldCanvas(false);
        }
        //if(gameState == Enum.GameState.Ingame && inGameState == Enum.InGameState.Zombie)
        //{
        //    Vector3 random = transformHolder[ Random.Range(0, transformHolder.Length)].position;
        //    transform.position = new Vector3(random.x, transform.position.y, random.y);
        //}
    }

    private void TurnWorldCanvas(bool check)
    {
        for (int i = 0; i < CanvasNeedToTurnOnOff.Length; i++)
        {
            CanvasNeedToTurnOnOff[i].gameObject.SetActive(check);
        }
    }
    
    public void PrepareForDestroy()
    {
        GameManager.Instance.SetGameStates(Enum.GameState.Revive, GameManager.Instance.CurrentInGameState);
    }
}
