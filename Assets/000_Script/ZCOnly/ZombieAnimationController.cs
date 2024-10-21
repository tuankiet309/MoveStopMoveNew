using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationController : ActorAnimationController
{
    [SerializeField] ZombieMovementController zombieController;
    protected override void OnEnable()
    {
        zombieController.onEnemyMoving.AddListener( UpdateMoveAnimation);
    }
    protected override void OnDisable()
    {
        zombieController .onEnemyMoving.RemoveAllListeners();
    }
    protected override void UpdateIsWon(Enum.GameState state, Enum.InGameState inGameState)
    {
        if (inGameState == Enum.InGameState.PVE)
            return;
        anim.SetBool("isWon",state==Enum.GameState.Dead || state==Enum.GameState.Revive);
    }
    protected override void UpdateMoveAnimation(Vector3 moveVec)
    {
        anim.SetBool("isMoving", moveVec != Vector3.zero);
    }
}
