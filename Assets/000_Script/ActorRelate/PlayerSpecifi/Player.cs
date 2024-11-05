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


    [Header("Essential Player Component")]
    public ActorAttacker attacker;
    public ActorAnimationController animationController;
    public ActorMovementController movementController;
    public ActorInformationController informationController;
    public ActorAtributeController atributeController;
    public WeaponComponent weaponComponent;
    public SkinComponent skinComponent;
    public LifeComponent lifeComponent;
    public DetectionCircle detectionCircle;

    public void Init()
    {
        attacker.InitAttacker(detectionCircle, weaponComponent, atributeController, null, animationController,movementController);
        atributeController.InitAttribute(attacker,movementController,informationController);
        movementController.InitMovementController(attacker, animationController, null);

        weaponComponent.InitWeapomComponent(attacker, atributeController);
        skinComponent.InitSkinComponent(atributeController);

    }
    public void PlayerAction()
    {
        movementController.Move();
    }
    public void PlayerLateAction()
    {
        attacker.LateUpdateCheck();
    }
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
        Init();
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
