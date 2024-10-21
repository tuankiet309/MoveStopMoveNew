using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAnimationController : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    [SerializeField] protected ActorMovementController actorMovementController;
    [SerializeField] protected EnemyMovementController enemyMovementController;
    [SerializeField] protected ActorAttacker attacker;
    [SerializeField] protected LifeComponent lifeComponent;
    [SerializeField] protected WeaponComponent weaponComponent;

    protected virtual void Start()
    {
        GameManager.Instance.onStateChange.AddListener(UpdateIsWon);
        GameManager.Instance.onStateChange.AddListener(UpdateIsDance);
    }

    

    protected virtual void OnEnable()
    {
        EnableEventListeners();
        EnableEventListeners();
    }
    protected virtual void OnDisable()
    {
        DisableEventListeners();
    }
    protected virtual void EnableEventListeners()
    {
        if (weaponComponent != null)
            weaponComponent.onHavingWeapon.AddListener(UpdateHavingWeapon);

        if (actorMovementController != null)
            actorMovementController.onActorMoving.AddListener(UpdateMoveAnimation);

        if (lifeComponent != null)
            lifeComponent.onLifeEnds.AddListener(UpdatePlayerDead);

        if (attacker != null)
        {
            attacker.onHaveTarget.AddListener(UpdateHaveTarget);
            attacker.onHaveUlti.AddListener(UpdateHaveUlti);
        }
        if (enemyMovementController != null)
            enemyMovementController.onEnemyMoving.AddListener(UpdateMoveAnimation);
    }
    protected virtual void DisableEventListeners()
    {
        if (weaponComponent != null)
            weaponComponent.onHavingWeapon.RemoveListener(UpdateHavingWeapon);

        if (actorMovementController != null)
            actorMovementController.onActorMoving.RemoveListener(UpdateMoveAnimation);

        if (lifeComponent != null)
            lifeComponent.onLifeEnds.RemoveListener(UpdatePlayerDead);

        if (attacker != null)
        {
            attacker.onHaveTarget.RemoveListener(UpdateHaveTarget);
            attacker.onHaveUlti.RemoveListener(UpdateHaveUlti);
        }
        if (enemyMovementController != null)
            enemyMovementController.onEnemyMoving.RemoveListener(UpdateMoveAnimation);
        if(GameManager.Instance != null)
            GameManager.Instance.onStateChange.RemoveListener(UpdateIsWon);
    }
    protected virtual void UpdateMoveAnimation(Vector3 moveVec)
    {
        anim.SetBool("isMoving", moveVec != Vector3.zero);
    }
    protected virtual void UpdatePlayerDead(string temp)
    {
        anim.SetTrigger("isDead");
        lifeComponent.onLifeEnds.RemoveListener(UpdatePlayerDead);
    }
    protected virtual void UpdateHaveTarget(GameObject target)
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
    protected virtual void UpdateHaveUlti(bool haveUlti)
    {
        anim.SetBool("haveUlti", haveUlti);
    }
    protected virtual void UpdateHavingWeapon(bool haveWeapon)
    {
        anim.SetBool("haveWeapon", haveWeapon);
    }
    protected virtual void UpdateIsWon(Enum.GameState gameState, Enum.InGameState inGameState)
    {
        if (gameState == Enum.GameState.Win)
        {
            anim.SetTrigger("isWon");
        }
    }
    protected virtual void UpdateIsDance(Enum.GameState state, Enum.InGameState inGameState)
    {
        anim.SetBool("isDance", state == Enum.GameState.SkinShop);
    }
}