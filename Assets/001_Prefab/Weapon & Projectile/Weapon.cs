using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon", order = 1)]
public class Weapon : ScriptableObject
{

    [SerializeField] private string idWeapon;

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
    private int tempoIndex = 0;



    public GameObject WeaponOnHand { get => weaponOnHand; private set { } }
    public Projectile WeaponThrowAway { get => weaponThrowAway; private set { } }
    public Vector3 WeaponOffsetPos { get => weaponOffsetPos; private set { } }
    public Quaternion WeaponOffsetRot { get => weaponOffsetRot; private set { } }
    public Enum.WeaponType WeaponType { get => weaponType; private set { } }
    public Enum.AttributeBuffs Buff { get => buff; private set { } }
    public float BuffMultiplyer { get => buffMultiplyer; private set { } }
    public SkinState[] PossibleSkinForThisWeapon { get => possibleSkinForThisWeapon; private set { } }
    public int CurrentIndexOfTheSkin { get => currentIndexOfTheSkin; set => currentIndexOfTheSkin = value; }
    public Vector3 WeaponOffsetOnThrow { get => weaponOffsetOnThrowPos; set => weaponOffsetOnThrowPos = value; }
    public string IdWeapon { get => idWeapon; set => idWeapon = value; }
    public int TempoIndex { get => tempoIndex; set => tempoIndex = value; }

    [ContextMenu("GenrateID")]
    private void GenerateGUID()
    {
        idWeapon = System.Guid.NewGuid().ToString();
    }
}

[System.Serializable]
public class SkinState
{
    [SerializeField] private GameObject skin;
    [SerializeField] private int gold = 200;
    [SerializeField] private bool isLocked = true;

    public GameObject Skin { get => skin; private set { } }

    public bool IsLocked { get => isLocked; set => isLocked = value; }
    public int Gold { get => gold; set => gold = value; }

    public void UnlockSkin()
    {
        isLocked = false;
    }
}
