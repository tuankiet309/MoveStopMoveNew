using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static Enum;

public class ZombieMovementController : MonoBehaviour
{
    private Player player;
    [SerializeField] private NavMeshAgent agent;

    public UnityEvent<Vector3> onEnemyMoving;

    [SerializeField] private float normalSpeed = 4.2f; 
    [SerializeField] private float doubleSpeed = 12.0f; 
    [SerializeField] private float detectionRange = 30f; 

    private void Start()
    {
        player = Player.Instance;
        GameManager.Instance.onStateChange.AddListener(OnMoving);

        if (agent != null)
        {
            agent.isStopped = true;
            agent.ResetPath();
            agent.SetDestination(player.transform.position);
        }
        OnMoving(GameManager.Instance.CurrentGameState, GameManager.Instance.CurrentInGameState);
    }

    private void OnEnable()
    {
        player = Player.Instance;
    }

    private void OnDisable()
    {
        if (agent != null)
        {
            agent.SetDestination(transform.position);
            agent.isStopped = true;
        }
    }

    private void Update()
    {
        if (agent != null && !agent.isStopped)
        {
            agent.SetDestination(player.transform.position);
            AdjustSpeedBasedOnDistance();
        }
    }

    private void AdjustSpeedBasedOnDistance()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        if (distanceToPlayer > detectionRange)
        {
            agent.speed = doubleSpeed; 
        }
        else
        {
            agent.speed = normalSpeed; 
        }
    }

    private void OnMoving(Enum.GameState gameState, Enum.GameplayState inGameState)
    {
        if (gameObject.activeInHierarchy == false)
            return;

        if (gameState == Enum.GameState.Ingame)
        {
            agent.isStopped = false;
            onEnemyMoving?.Invoke(Vector3.one);
        }
        else if (gameState == Enum.GameState.Begin)
        {
            agent.isStopped = false;
            onEnemyMoving?.Invoke(Vector3.one);
        }
        else if (gameState == Enum.GameState.Dead || gameState == Enum.GameState.Revive)
        {
            agent.isStopped = true;
            onEnemyMoving?.Invoke(Vector3.zero);
        }
    }
}