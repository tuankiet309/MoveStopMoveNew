using System.Collections;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    [SerializeField] private Transform attachedLocation;
    [SerializeField] private Weapon weapon;
    [SerializeField] private ActorAttacker attacker;

    private GameObject weaponOnHand;
    private Projectile weaponOffHand;

    public delegate void OnHavingWeapon(bool isHavingWeapon);
    public event OnHavingWeapon onHavingWeapon;

    private void Start()
    {
        InitWeapon();
    }

    public void AssignWeapon(Weapon newWeapon)
    {
        weapon = newWeapon;
        InitWeapon();
    }

    private void InitWeapon()
    {
        if (weapon == null) return;

        ClearOldWeapon();
        CreateWeaponOnHand();
        SetupWeaponOffHand();

        attacker.InitWeapon(weaponOffHand);
        attacker.onActorAttack += OnThrowAwayWeapon;
        onHavingWeapon?.Invoke(true);
    }

    private void ClearOldWeapon()
    {
        if (weaponOnHand) Destroy(weaponOnHand.gameObject);
        if (weaponOffHand) Destroy(weaponOffHand.gameObject);
    }

    private void CreateWeaponOnHand()
    {
        weaponOnHand = Instantiate(weapon.WeaponOnHand, attachedLocation);
        weaponOnHand.transform.localPosition = weapon.WeaponOffsetPos;
        weaponOnHand.transform.localRotation = weapon.WeaponOffsetRot;
        ApplyWeaponSkin(weaponOnHand, weapon.CurrentIndexOfTheSkin);
    }
    private void SetupWeaponOffHand()
    {
        weaponOffHand = Instantiate(weapon.WeaponThrowAway, transform);
        weaponOffHand.transform.rotation = Quaternion.Euler(Vector3.zero);
        weaponOffHand.gameObject.SetActive(false);
        GameObject weaponVisualize = Instantiate(weapon.WeaponOnHand, weaponOffHand.transform);
        weaponVisualize.transform.localPosition = weapon.WeaponOffsetOnThrow;
        ApplyWeaponSkin(weaponVisualize, weapon.CurrentIndexOfTheSkin);
        weaponOffHand.SetUpWeapon(weapon.WeaponType,weapon.BuffMultiplyer,weapon.Buff);
    }

    private void ApplyWeaponSkin(GameObject weaponObject, int skinIndex)
    {
        MeshRenderer meshRenderer = weaponObject.GetComponent<MeshRenderer>();
        if (meshRenderer != null && weapon.PossibleSkinForThisWeapon.Length > skinIndex)
        {
            meshRenderer.sharedMaterials = weapon.PossibleSkinForThisWeapon[skinIndex].Skin.GetComponent<MeshRenderer>().sharedMaterials;
        }
    }
    private void OnThrowAwayWeapon(Vector2 hold)
    {
        if (weaponOnHand == null) return;
        weaponOnHand.SetActive(false);
        onHavingWeapon?.Invoke(false);
        StartCoroutine(GetWeaponBack());
    }
    private IEnumerator GetWeaponBack()
    {
        yield return new WaitForSeconds(CONSTANT_VALUE.FIRST_DELAYED_ATTACK + (weapon.Buff == Enum.AttributeBuffs.AttackSpeed ? weapon.BuffMultiplyer : 0) );
        if (weaponOnHand != null)
        {
            weaponOnHand.SetActive(true);
            onHavingWeapon?.Invoke(true);
        }
    }
}