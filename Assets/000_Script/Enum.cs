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
        //---------------Will Delete Later------------
        Ingame,
        Setting,
        Hall,
        SkinShop,
        Dead,
        Win,
        Ads,
        Revive,
        Begin,


        HallState,
        GameplayState,
        ZombieState
    }

    public enum GameplayState
    {
        //---------------Will Delete Later------------
        PVE,
        Zombie,

        WinState,
        DeadState,
        ReviveState
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
    public enum ZCUpgradeType 
    {
        Protect,
        Speed,
        CircleRange,
        MaxWeapon,
    }
    public enum ZCPowerUp
    {
        Continous,
        AttackBehind,
        BulletPlus,
        PursueBullet,
        CrossAttack,
        DiaAttack,
        DoubleGold,
        GrowWeapon,
        MoveFaster,
        PieceWeapon,
        Revive,
        Bigger,
        Tripple,
        IgnoreWall
    }

    public enum SoundType
    {
        UIButton,
        UISlider,
        DoneGame,
        WeaponSound,
        Dead,
        SizeUp,
        CountDown
    }

    public enum SceneName
    {
        LoadingScene,
        PVEScene,
        ZCScene
    }    
}
