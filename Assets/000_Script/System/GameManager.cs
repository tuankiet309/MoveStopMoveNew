using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } private set { } }
    private Enum.GameState currentGameState;
    private Enum.InGameState currentInGameState;
    public UnityEvent<Enum.GameState> onStateChange;
    public UnityEvent<Enum.InGameState> onInGameStateChange;

    public Enum.GameState CurrentGameState { get => currentGameState; private set { } }

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        currentGameState = Enum.GameState.Hall;

        DontDestroyOnLoad(gameObject);
        
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
    public void SetCurrentInGame(Enum.InGameState inGameState)
    {
        currentInGameState = inGameState;
        onInGameStateChange?.Invoke(currentInGameState);
    }



}
