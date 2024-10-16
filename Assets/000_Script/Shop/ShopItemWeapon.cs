using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Shop/WeaponItem", order = 1)]

public class ShopItemWeapon : ScriptableObject
{
    [SerializeField] private Weapon weapon;
    [SerializeField] private string weaponName;
    [SerializeField] private int goldCost;
    [SerializeField] private bool isPurchased;
    public Weapon Weapon { get => weapon; set => weapon = value; }
    public string WeaponName { get => weaponName; set => weaponName = value; }
    public int GoldCost { get => goldCost; set => goldCost = value; }
    public bool IsPurchased { get => isPurchased; set => isPurchased = value; }
}
