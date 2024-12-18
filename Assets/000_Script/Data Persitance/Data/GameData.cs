using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    ///Game essentials
    public int gold;
    public int maxScore;
    public int level;
    public int currentExp;
    ///Player information
    public PlayerData playerData;
    public List<SkinShopItemData> skinDatas = new List<SkinShopItemData>();
    public List<WeaponShopItemData> weaponDatas = new List<WeaponShopItemData>();
    public List<StatData> statDatas = new List<StatData>();
    public List<StatItemData> statItemDatas = new List<StatItemData>();
    public LevelData levelData;
    public SettingData settingData;
    public GameData()
    {
        this.gold = 0;
        this.maxScore = 0;
        level = 0;
        currentExp = 0;
        this.playerData = new PlayerData();
        settingData = new SettingData();
    }
}

[System.Serializable]
public class PlayerData
{
    public string playerName;
    public string[] playerCurrentWearingSkinID;
    public bool isASet;
    public string playerCurrentWearingWeaponID;
    public int currentIndexOfTheWeaponSkinPlayerWearing;
    public PlayerData()
    {
        playerName = "Player1";
        currentIndexOfTheWeaponSkinPlayerWearing = 1;
    }
    public void InitializePlayerData(string defaultWeapon, string[] defaultSkin, bool isASet)
    {
        playerCurrentWearingWeaponID = defaultWeapon;
        playerCurrentWearingSkinID = defaultSkin;
        this.isASet = isASet;
        currentIndexOfTheWeaponSkinPlayerWearing = 1;
    }
}
[System.Serializable]
public class WeaponShopItemData
{
    public string id;
    public bool isPurchased;
    public int timeToWatchAdToPuchase;
    public List<int> skinArePurchased;
    public void InitializeWeaponData(string indexOfWeapon, bool isPurchase, int times, List<int> skinArePurchased)
    {
        this.id = indexOfWeapon;
        this.isPurchased = isPurchase;
        this.skinArePurchased = skinArePurchased;
        this.timeToWatchAdToPuchase = times;
    }
}

[System.Serializable]
public class SkinShopItemData
{
    public string idOfSkin;
    public bool isPurchased;
    public bool isUnlockOnce;
    public bool isUseYet;
    public void InitializeSkinData(string idOfSkin, bool isPurchased,bool isUnlockedOnce, bool isUseYet)
    {
        this.idOfSkin = idOfSkin;
        this.isPurchased= isPurchased;
        this.isUnlockOnce = isUnlockedOnce;
        this.isUseYet = isUseYet;
    }
}


[System.Serializable]
public class StatData
{
    public Enum.ZCUpgradeType type;
    public int statNumber;

    public void InitStatData(int statNumber, Enum.ZCUpgradeType type)
    {
        this.type = type;
        this.statNumber = statNumber;
    }
}
[System.Serializable]
public class StatItemData
{
    public Enum.ZCUpgradeType type;
    public int level;
    public void InitStatItemData(int level, Enum.ZCUpgradeType type)
    {
        this.type = type;
        this.level = level;
    }
}

[System.Serializable]
public class LevelData
{
    public int currentPVELevel;
    public int currentZCLevel;
    public LevelData()
    {
        currentPVELevel = 0;
        currentZCLevel = 0;
    }
}
[System.Serializable]
public class SettingData
{
    public bool isMute;
    public bool isUnvibrate;
    public SettingData() 
    {
        isMute = false;
        isUnvibrate = false;
    }
}