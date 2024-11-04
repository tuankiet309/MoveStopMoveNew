using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour, IPoolable
{
    public static int numberOfEnemyRightnow = 0;
    public static int numberOfEnemyHasDie = 0;
    private EnemyPool pool;
    private Material currentSkin;

    public UnityEvent onEnemyDie;
    private void OnEnable()
    {
        numberOfEnemyRightnow++;

    }

    private void OnDisable()
    {
        
        numberOfEnemyRightnow--;
        numberOfEnemyHasDie++;
        
    }

    public void PrepareForDestroy()
    {
        onEnemyDie?.Invoke();
        pool.Release(this);
        onEnemyDie.RemoveAllListeners();
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
    public void New()
    {
    }

    public void Free()
    {
    }
}