using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } private set { } }
    private Enum.GameState currentGameState;

    public UnityEvent<Enum.GameState> onStateChange;

    public Enum.GameState CurrentGameState { get => currentGameState; private set { } }

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        currentGameState = Enum.GameState.Hall;
        
    }
    private void Start()
    {
        onStateChange?.Invoke(currentGameState);
    }
    public void SetGameState(Enum.GameState gameState)
    {
        currentGameState = gameState;
        onStateChange?.Invoke(currentGameState);  
    }



}
