using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } private set { } }
    [SerializeField]private Enum.GameState currentGameState = Enum.GameState.Hall;
    [SerializeField]private Enum.InGameState currentInGameState = Enum.InGameState.PVE;
    public UnityEvent<Enum.GameState,Enum.InGameState> onStateChange;
    

    public Enum.GameState CurrentGameState { get => currentGameState; private set { } }
    public Enum.InGameState CurrentInGameState { get => currentInGameState; set => currentInGameState = value; }

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
   
    }
    private void Start()
    {
        onStateChange?.Invoke(currentGameState, currentInGameState);
        Application.targetFrameRate = 60;

    }
    public void SetGameStates(Enum.GameState gameState, Enum.InGameState inGameState)
    {
        currentGameState = gameState;
        currentInGameState = inGameState;
        onStateChange?.Invoke(currentGameState,currentInGameState);  
    }


}
