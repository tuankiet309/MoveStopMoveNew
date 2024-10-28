using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZCStatUpgradeUI : MonoBehaviour,IDataPersistence
{
    [SerializeField] ZCStatItem item;
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI gold;
    [SerializeField] TextMeshProUGUI buffDes;
    [SerializeField] Button thisButton;

    private void Start()
    {
        UpdateItemInfo();
    }
    private void OnEnable()
    {
        LoadData(DataPersistenceManager.Instance.GameData);

    }
    private void OnDisable()
    {
        GameData game = DataPersistenceManager.Instance.GameData;
        SaveData(ref game);
    }
    private void OnApplicationQuit()
    {
        GameData game = DataPersistenceManager.Instance.GameData;
        SaveData(ref game);
    }
    private void UpdateItemInfo()
    {
        if (item.Level == item.Stat.Length)
        {
            image.sprite = item.StateImage;
            gold.text = "Maximum";
            switch (item.Type)
            {
                case Enum.ZCUpgradeType.Protect:
                    buffDes.text = item.Stat[item.Stat.Length-1].HowMuchUpgrade.ToString() + " times";
                    break;
                case Enum.ZCUpgradeType.Speed:
                    buffDes.text = "+" + (item.Stat[item.Stat.Length - 1].HowMuchUpgrade ).ToString() + "% Speed";
                    break;
                case Enum.ZCUpgradeType.MaxWeapon:
                    buffDes.text = "Max: " + item.Stat[item.Stat.Length - 1].HowMuchUpgrade.ToString();
                    break;
                case Enum.ZCUpgradeType.CircleRange:
                    buffDes.text = "+" + (item.Stat[item.Stat.Length - 1].HowMuchUpgrade ).ToString() + "% Range";
                    break;
            }
            thisButton.interactable = false;
        }
        else
        {
            image.sprite = item.StateImage;
            gold.text = item.Stat[item.Level].Price.ToString();
            switch (item.Type)
            {
                case Enum.ZCUpgradeType.Protect:
                    buffDes.text = item.Stat[item.Level].HowMuchUpgrade.ToString() + " times";
                    break;
                case Enum.ZCUpgradeType.Speed:
                    buffDes.text = "+" + (item.Stat[item.Level].HowMuchUpgrade).ToString() + "% Speed";
                    break;
                case Enum.ZCUpgradeType.MaxWeapon:
                    buffDes.text = "Max: " + item.Stat[item.Level].HowMuchUpgrade.ToString();
                    break;
                case Enum.ZCUpgradeType.CircleRange:
                    buffDes.text = "+" + (item.Stat[item.Level].HowMuchUpgrade).ToString() + "% Range";
                    break;
            }

            thisButton.onClick.AddListener(Add);
        }
    }

    private void Add()
    {
        if (item.Level == item.Stat.Length)
            return;
        if (DataPersistenceManager.Instance.AccessGold(-item.Stat[item.Level].Price))
        {
            Player.Instance.GetComponent<ZCAttributeController>().UpgradeStat(item.Stat[item.Level]);
            item.Level++;
            UpdateItemInfo();
        }
    }

    public void LoadData(GameData gameData)
    {
        
        StatItemData itemData = gameData.statItemDatas.Find(stat => stat.type == item.Type);
        if(itemData != null)
        {
            item.Level = itemData.level;
        }
        else
        {
            item.Level = 0;
            StatItemData newItem = new StatItemData();
            newItem.type = item.Type;
            newItem.level = item.Level;
            gameData.statItemDatas.Add(newItem);
        }
    }

    public void SaveData(ref GameData gameData)
    {
        StatItemData itemData = gameData.statItemDatas.Find(stat => stat.type == item.Type);
        if (itemData != null)
        {
            itemData.level = item.Level;
        }
        else
        {
            StatItemData newItem = new StatItemData();
            newItem.type = item.Type;
            newItem.level = item.Level;
            gameData.statItemDatas.Add(newItem);
        }
    }
}
