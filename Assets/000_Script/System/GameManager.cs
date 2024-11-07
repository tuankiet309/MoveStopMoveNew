using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GameManager : MonoBehaviour
{
    [Header("___________________________________GameManager____________________________________")]
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } private set { } }


    [Header("______________________________Optional Manager___________________________________")]
    public GameplayManager gameplayManager;
    public HallManager hallManager;
    public ZCManager zcManager;


    [Header("______________________________Multi Scene Manager________________________________")]
    public ParticleManager particleManager;
    public CameraManager cameraManager;
    public LevelManager levelManager;
    public SceneController sceneController;
    public SoundManager soundManager;

    [Header("__________________________________Data Relate_____________________________________")]
    public DataPersistenceManager dataPersistenceManager;
    [Space]


    //!!!!!!!!!!!!!!!!!!!!!!!!!!  WILL FIX LATER !!!!!!!!!!!!!!!!!!!!!!!
    private Enum.GameState currentGameState;
    private Enum.GameplayState currentInGameState = Enum.GameplayState.PVE;
    public UnityEvent<Enum.GameState,Enum.GameplayState> onStateChange;
    public Enum.GameState CurrentGameState { get => currentGameState; private set { } }
    public Enum.GameplayState CurrentInGameState { get => currentInGameState; set => currentInGameState = value; }
    //!!!!!!!!!!!!!!!!!!!!!!!!!! END !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
        soundManager.InitSoundManager();
        particleManager.InitParticleManager();
        cameraManager.InitCameraManager();
        levelManager.Init();
    }
    public void SetGameStates(Enum.GameState gameState, Enum.GameplayState inGameState)
    {
        //currentGameState = gameState;
        //currentInGameState = inGameState;
        //onStateChange?.Invoke(currentGameState,currentInGameState);  
    }

    public void UpdateGameState(Enum.GameState gameState)
    {
        Enum.GameState thisGameState = gameState;
        switch (thisGameState) 
        {
            case Enum.GameState.HallState:
                HallManager.Instance.Init();
                GameplayManager.Instance.Init();
                hallManager.hallUI.gameObject.SetActive(true);
                break;
            case Enum.GameState.GameplayState:
                gameplayManager.InitGameplay();
                break;


        }
    }


}
