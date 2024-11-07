using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    private EnemyPool pool;
    private Material currentSkin;


    [Space]
    [Header("Essential Enemy Component")]
    public ActorAttacker attacker;
    public ActorAnimationController animationController;
    public EnemyMovementController movementController;
    public ActorInformationController informationController;
    public ActorAtributeController atributeController;
    public WeaponComponent weaponComponent;
    public SkinComponent skinComponent;
    public LifeComponent lifeComponent;
    public DetectionCircle detectionCircle;

    public void Init()
    {
        attacker.InitAttacker(detectionCircle, weaponComponent, atributeController, movementController, animationController, null);
        atributeController.InitAttribute(attacker, null, informationController,movementController);
        movementController.InitEMovementController(animationController);

        lifeComponent.InitLifeComponent(animationController);
        weaponComponent.InitWeapomComponent(attacker, atributeController, animationController, movementController);
        skinComponent.InitSkinComponent(atributeController);

    }
    public void EnemyAction()
    {
        movementController.EnemyMove();
    }
    public void EnemyLateAction()
    {
        attacker.CheckAndUpdateTargetCircle();
    }

    public void PrepareForDestroy()
    {
        pool.Release(this);
    }
    public void Initialize(Skin pant, Skin body, Skin head, Skin leftHand, Weapon weapon, string name, EnemyPool pool)
    {
        SkinComponent skinController = GetComponent<SkinComponent>();
        WeaponComponent weaponComponent = GetComponent<WeaponComponent>();
        EnemyMovementController movementController = GetComponent<EnemyMovementController>();
        ActorInformationController actorInfomationController = GetComponent<ActorInformationController>();
        int randomWeaponSkin = Random.Range(1, weapon.PossibleSkinForThisWeapon.Length);
        weapon.TempoIndex = randomWeaponSkin;
        if (skinController != null)
        {
            skinController.AssignNewSkin( new Skin[] { pant,body,head,leftHand} , false);
        }
        if (weaponComponent != null)
        {
            weaponComponent.AssignWeapon(weapon);
            weaponComponent.AssignWeapon(weapon);
        }

        if (actorInfomationController != null)
        {
            actorInfomationController.UpdateName(name);
        }
        if(pool != null)
            this.pool = pool;
    }

}