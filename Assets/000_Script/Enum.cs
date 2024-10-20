using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enum : MonoBehaviour
{
    public enum WeaponType
    {
        Straight,  // Ball
        Rotate,    // Axe
        Comeback   // boomerang
    }
    public enum GameState
    {
        Ingame,
        Hall,
        SkinShop,
        Dead,
        Win,
        Ads,
        Revive
    }

    public enum InGameState
    {
        Zone1,
        Zone2,
        Zombie
    }
    public enum AttributeBuffs
    {
        Range,
        Speed,
        AttackSpeed,
        Gold
    }

    public enum SkinType
    {
        Hair,
        Pant,
        LHand,
        Tail,
        Wing,
        Body,
        Set
    }
}
