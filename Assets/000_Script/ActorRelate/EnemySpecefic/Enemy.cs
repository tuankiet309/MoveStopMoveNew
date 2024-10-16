using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour, IPoolable
{
    public static int numberOfEnemyRightnow = 0;
    public static int numberOfEnemyHasDie = 0;
    private EnemyPool pool;
    private Material currentSkin;
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
        EnemySpawner.Instance.ReturnSkinToPool(currentSkin);
        pool.Release(this);
        
    }
    public void Initialize(Material skinColor, Material pantColor, Weapon weapon, string name, EnemyPool pool)
    {
        EnemySkinController skinController = GetComponent<EnemySkinController>();
        WeaponComponent weaponComponent = GetComponent<WeaponComponent>();
        EnemyMovementController movementController = GetComponent<EnemyMovementController>();
        ActorInformationController actorInfomationController = GetComponent<ActorInformationController>();
        int randomWeaponSkin = Random.Range(1, weapon.PossibleSkinForThisWeapon.Length);
        weapon.CurrentIndexOfTheSkin = randomWeaponSkin;
        if (skinController != null)
        {
            skinController.ChangeSkin(skinColor, pantColor);
            currentSkin = skinController.SkinToChanged.material;
        }
        if (weaponComponent != null)
        {
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