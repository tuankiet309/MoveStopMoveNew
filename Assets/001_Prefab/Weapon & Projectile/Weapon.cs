using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon", order = 1)]
public class Weapon : ScriptableObject
{
    //Cac thong so can chinh sua trong editor
    [SerializeField] GameObject weaponOnHand;
    [SerializeField] Projectile weaponThrowAway;

    [SerializeField] SkinState[] possibleSkinForThisWeapon;
    [SerializeField] Vector3 weaponOffsetPos;
    [SerializeField] Quaternion weaponOffsetRot;
    [SerializeField] Vector3 weaponOffsetOnThrowPos;
    [SerializeField] Enum.WeaponType weaponType;
    [SerializeField] Enum.AttributeBuffs buff;
    [SerializeField] float buffMultiplyer;
    [SerializeField] private int currentIndexOfTheSkin = 0;


    float range = 0f;
    private void OnEnable()
    {

    }
    public GameObject WeaponOnHand { get => weaponOnHand; private set { } }
    public Projectile WeaponThrowAway { get => weaponThrowAway; private set { } }
    public Vector3 WeaponOffsetPos { get => weaponOffsetPos; private set { } }
    public Quaternion WeaponOffsetRot { get => weaponOffsetRot; private set { } }
    public Enum.WeaponType WeaponType { get => weaponType; private set { } }
    public Enum.AttributeBuffs Buff { get => buff; private set { } }
    public float BuffMultiplyer { get => buffMultiplyer; private set { } }
    public float Range { get => range; private set { } }
    public SkinState[] PossibleSkinForThisWeapon { get => possibleSkinForThisWeapon; private set { } }
    public int CurrentIndexOfTheSkin { get => currentIndexOfTheSkin; set => currentIndexOfTheSkin = value; }
    public Vector3 WeaponOffsetOnThrow { get => weaponOffsetOnThrowPos; set => weaponOffsetOnThrowPos = value; }
}

[System.Serializable]
public class SkinState
{
    [SerializeField] private GameObject skin;
    [SerializeField] private bool isLocked = true;

    public GameObject Skin { get => skin; private set { } }

    public bool IsLocked { get => isLocked; set => isLocked = value; }

    public void UnlokSkin()
    {
        isLocked = false;
    }
}
