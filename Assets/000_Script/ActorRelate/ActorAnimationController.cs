using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAnimationController : MonoBehaviour
{
    [SerializeField] protected Animator anim;

    public virtual void UpdateMoveAnimation(Vector3 moveVec)
    {
        anim.SetBool("isMoving", moveVec != Vector3.zero);
    }
    public virtual void UpdatePlayerDead()
    {
        anim.SetTrigger("isDead");
    }
    public virtual void UpdateHaveTarget(GameObject target)
    {
        if (target != null)
        {
            anim.SetBool("haveEnemy", true);
        }
        else
        {
            anim.SetBool("haveEnemy", false);
        }
    }
    public virtual void UpdateHaveUlti(bool haveUlti)
    {
        anim.SetBool("haveUlti", haveUlti);
    }
    public virtual void UpdateHavingWeapon(bool haveWeapon)
    {
        anim.SetBool("haveWeapon", haveWeapon);
    }
    public virtual void UpdateIsWon(Enum.GameState gameState, Enum.GameplayState inGameState)
    {
        if (gameState == Enum.GameState.Win)
        {
            anim.SetTrigger("isWon");
        }
    }
    public virtual void UpdateIsDance(Enum.GameState state, Enum.GameplayState inGameState)
    {
        anim.SetBool("isDance", state == Enum.GameState.SkinShop);
    }
}