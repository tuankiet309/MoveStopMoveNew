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
        PrepareGamestate(Enum.GameState.Hall);
    }
    private void PrepareGamestate(Enum.GameState gameState)
    {
        if (gameState == Enum.GameState.Hall )
        {
            transform.position = new Vector3(0, transform.position.y, 0);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            TurnWorldCanvas(false);


        }
        if (gameState == Enum.GameState.Ingame)
        {
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
        GameManager.Instance.SetGameState(Enum.GameState.Revive);
    }
}
