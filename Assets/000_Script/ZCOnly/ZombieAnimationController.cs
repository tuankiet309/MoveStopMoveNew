using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationController : ActorAnimationController
{
    [SerializeField] ZombieMovementController zombieController;
    //public override void OnEnable()
    //{
    //    zombieController.onEnemyMoving.AddListener( UpdateMoveAnimation);
    //}
    //public override void OnDisable()
    //{
    //    zombieController .onEnemyMoving.RemoveAllListeners();
    //}
    public override void UpdateIsWon(Enum.GameState state, Enum.GameplayState inGameState)
    {
        if (inGameState == Enum.GameplayState.PVE)
            return;
        anim.SetBool("isWon",state==Enum.GameState.Dead || state==Enum.GameState.Revive);
    }
    public override void UpdateMoveAnimation(Vector3 moveVec)
    {
        anim.SetBool("isMoving", moveVec != Vector3.zero);
    }
}
