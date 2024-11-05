using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorAnimationController : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    protected EnemyMovementController enemyMovementController;
    protected ActorAttacker attacker;
    protected WeaponComponent weaponComponent;

    public virtual void Start()
    {
        GameManager.Instance.onStateChange.AddListener(UpdateIsWon);
        GameManager.Instance.onStateChange.AddListener(UpdateIsDance);
    }
    public virtual void OnEnable()
    {
        EnableEventListeners();
        EnableEventListeners();
    }
    public virtual void OnDisable()
    {
        DisableEventListeners();
    }
    public virtual void EnableEventListeners()
    {
        if (weaponComponent != null)
            weaponComponent.onHavingWeapon.AddListener(UpdateHavingWeapon);



        if (enemyMovementController != null)
            enemyMovementController.onEnemyMoving.AddListener(UpdateMoveAnimation);
    }
    public virtual void DisableEventListeners()
    {
        if (weaponComponent != null)
            weaponComponent.onHavingWeapon.RemoveListener(UpdateHavingWeapon);
        if (enemyMovementController != null)
            enemyMovementController.onEnemyMoving.RemoveListener(UpdateMoveAnimation);
        if(GameManager.Instance != null)
            GameManager.Instance.onStateChange.RemoveListener(UpdateIsWon);
    }
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
    public virtual void UpdateIsWon(Enum.GameState gameState, Enum.InGameState inGameState)
    {
        if (gameState == Enum.GameState.Win)
        {
            anim.SetTrigger("isWon");
        }
    }
    public virtual void UpdateIsDance(Enum.GameState state, Enum.InGameState inGameState)
    {
        anim.SetBool("isDance", state == Enum.GameState.SkinShop);
    }
}