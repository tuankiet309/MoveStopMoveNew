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

    [Space]
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
        atributeController.InitAttribute(attacker,movementController,informationController,null);
        movementController.InitMovementController(attacker, animationController, null);
        lifeComponent.InitLifeComponent(animationController);
        
        weaponComponent.InitWeapomComponent(attacker, atributeController,animationController,null);
        skinComponent.InitSkinComponent(atributeController);
        PrepareGamestate(GameManager.Instance.CurrentGameState);
    }
    public void PlayerAction()
    {
        movementController.Move();
    }
    public void PlayerLateAction()
    {
        attacker.CheckAndUpdateTargetCircle();
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

    public void PrepareGamestate(Enum.GameState gameState)
    {

        if (gameState == Enum.GameState.HallState)
        {
            transform.position = new Vector3(0, transform.position.y, 0);
            transform.rotation = Quaternion.Euler(0, 180, 0);
            TurnWorldCanvas(false);
        }
        if (gameState == Enum.GameState.GameplayState)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            TurnWorldCanvas(true);
        }
        if (gameState == Enum.GameState.ZombieState)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            TurnWorldCanvas(true);
        }
    }

    public void PrepareGameplayState(Enum.GameState gameState)
    {
        if (gameState == Enum.GameState.Dead)
        {
            TurnWorldCanvas(false);
        }
        if (gameState == Enum.GameState.Win)
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
