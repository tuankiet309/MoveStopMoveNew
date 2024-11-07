using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WeaponComponent : MonoBehaviour,IDataPersistence
{
    [SerializeField] private Transform attachedLocation;
    [SerializeField] private Weapon weapon;

    private ActorAttacker attacker;
    private ActorAtributeController actorAtributeController;
    private ActorAnimationController actorAnimationController;
    private EnemyMovementController enemyMovementController;

    public UnityEvent<bool> onHavingWeapon;
    bool isInZC = false;

    public void InitWeapomComponent(ActorAttacker actorAttacker, ActorAtributeController actorAtribute,
        ActorAnimationController actorAnimation, EnemyMovementController enemyMovementController)
    {
        attacker = actorAttacker;
        actorAtributeController = actorAtribute;
        actorAnimationController = actorAnimation;
        this.enemyMovementController = enemyMovementController;
        AssignWeapon(weapon);
    }

    public void AssignWeapon(Weapon newWeapon)
    {  
        attacker.InitWeapon(newWeapon);
        actorAtributeController.ApplyBuffByWeapon(weapon, newWeapon);
        weapon = newWeapon;
        EventsRelateToHavingWeapon(true);
    }
    public void ApplyWeaponSkin(GameObject weaponObject, int skinIndex)
    {
        MeshRenderer meshRenderer = weaponObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null && weapon.PossibleSkinForThisWeapon.Length > skinIndex)
        {
            meshRenderer.sharedMaterials = weapon.PossibleSkinForThisWeapon[skinIndex].Skin.GetComponent<MeshRenderer>().sharedMaterials;
        }
    }
    public void OnThrowAwayWeapon()
    {
        EventsRelateToHavingWeapon(false);
        StartCoroutine(GetWeaponBack());
    }
    private IEnumerator GetWeaponBack()
    {
        if(isInZC)
            yield return new WaitForSeconds(CONSTANT_VALUE.FIRST_DELAYED_ATTACKZC + (weapon.Buff == Enum.AttributeBuffs.AttackSpeed ? weapon.BuffMultiplyer : 0));
        else
            yield return new WaitForSeconds(CONSTANT_VALUE.FIRST_DELAYED_ATTACK + (weapon.Buff == Enum.AttributeBuffs.AttackSpeed ? weapon.BuffMultiplyer : 0) );
        EventsRelateToHavingWeapon(true);

    }
    private void EventsRelateToHavingWeapon(bool checkThrow)
    {
        attachedLocation.gameObject.SetActive(checkThrow);
        actorAnimationController.UpdateHavingWeapon(checkThrow);
        if (enemyMovementController != null) 
        {
            enemyMovementController.OnHavingWeapon(checkThrow);
        }
    }
    public void LoadData(GameData gameData)
    {
        
        weapon = DataPersistenceManager.Instance.WeaponDatabase[gameData.playerData.playerCurrentWearingWeaponID];
        weapon.CurrentIndexOfTheSkin = gameData.playerData.currentIndexOfTheWeaponSkinPlayerWearing;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.playerData.playerCurrentWearingWeaponID = weapon.IdWeapon;
        gameData.playerData.currentIndexOfTheWeaponSkinPlayerWearing = weapon.CurrentIndexOfTheSkin;
    }
}