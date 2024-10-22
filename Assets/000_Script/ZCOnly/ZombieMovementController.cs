using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static Enum;

public class ZombieMovementController : MonoBehaviour
{
    private Player player;
    [SerializeField]private NavMeshAgent agent;

    public UnityEvent<Vector3> onEnemyMoving;

    private void Start()
    {
        player = Player.Instance;
        GameManager.Instance.onStateChange.AddListener(OnMoving);
        OnMoving(GameManager.Instance.CurrentGameState, GameManager.Instance.CurrentInGameState);
    }
    private void Update()
    {
        agent.SetDestination(player.transform.position);
    }
    private void OnMoving(Enum.GameState gameState, Enum.InGameState ingameState)
    {
        if (gameState == Enum.GameState.Ingame)
        {
            agent.isStopped = true;
            onEnemyMoving?.Invoke(Vector3.zero);
        }
        if(gameState == Enum.GameState.Begin)
        {
            agent.isStopped = false;
            onEnemyMoving?.Invoke(Vector3.one);
        }
    }
    
}
