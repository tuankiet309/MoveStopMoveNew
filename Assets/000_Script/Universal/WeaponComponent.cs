using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WeaponComponent : MonoBehaviour,IDataPersistence
{
    [SerializeField] private Transform attachedLocation;
    [SerializeField] private Weapon weapon;
    [SerializeField] private ActorAttacker attacker;
    
    public UnityEvent<bool> onHavingWeapon;
    public UnityEvent<Weapon,Weapon> onAssignNewWeapon;

    bool isInZC;

    private void Start()
    {
        AssignWeapon(weapon);
        isInZC = GameManager.Instance.CurrentInGameState == Enum.InGameState.Zombie;
    }

    public void AssignWeapon(Weapon newWeapon)
    {  
        onAssignNewWeapon?.Invoke(weapon,newWeapon);
        weapon = newWeapon;
        InitWeapon();
    }

    private void InitWeapon()
    {
        if (weapon == null) return;
        attacker.onActorAttack.AddListener(OnThrowAwayWeapon);
        onHavingWeapon?.Invoke(true);
    }

    public void ApplyWeaponSkin(GameObject weaponObject, int skinIndex)
    {
        MeshRenderer meshRenderer = weaponObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null && weapon.PossibleSkinForThisWeapon.Length > skinIndex)
        {
            meshRenderer.sharedMaterials = weapon.PossibleSkinForThisWeapon[skinIndex].Skin.GetComponent<MeshRenderer>().sharedMaterials;
        }
    }
    private void OnThrowAwayWeapon(Vector2 hold)
    {
        attachedLocation.gameObject.SetActive(false);
        onHavingWeapon?.Invoke(false);
        StartCoroutine(GetWeaponBack());
    }
    private IEnumerator GetWeaponBack()
    {
        if(isInZC)
            yield return new WaitForSeconds(CONSTANT_VALUE.FIRST_DELAYED_ATTACKZC + (weapon.Buff == Enum.AttributeBuffs.AttackSpeed ? weapon.BuffMultiplyer : 0));
        else
            yield return new WaitForSeconds(CONSTANT_VALUE.FIRST_DELAYED_ATTACK + (weapon.Buff == Enum.AttributeBuffs.AttackSpeed ? weapon.BuffMultiplyer : 0) );
        attachedLocation.gameObject.SetActive(true);
        onHavingWeapon?.Invoke(true);

    }

    public void LoadData(GameData gameData)
    {
        
        weapon = DataPersistenceManager.Instance.WeaponDatabase[gameData.playerData.playerCurrentWearingWeaponID];
        weapon.CurrentIndexOfTheSkin = gameData.playerData.currentIndexOfTheWeaponSkinPlayerWearing;
        AssignWeapon(weapon);
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.playerData.playerCurrentWearingWeaponID = weapon.IdWeapon;
        gameData.playerData.currentIndexOfTheWeaponSkinPlayerWearing = weapon.CurrentIndexOfTheSkin;
    }
}