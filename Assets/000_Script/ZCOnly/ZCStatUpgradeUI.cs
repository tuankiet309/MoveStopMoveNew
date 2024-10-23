using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ZCStatUpgradeUI : MonoBehaviour
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
                    buffDes.text = "+" + (item.Stat[item.Level].HowMuchUpgrade / 100).ToString() + "% Speed";
                    break;
                case Enum.ZCUpgradeType.MaxWeapon:
                    buffDes.text = "Max: " + item.Stat[item.Level].HowMuchUpgrade.ToString();
                    break;
                case Enum.ZCUpgradeType.CircleRange:
                    buffDes.text = "+" + (item.Stat[item.Level].HowMuchUpgrade / 100).ToString() + "% Range";
                    break;
            }

            thisButton.onClick.AddListener(Add);
        }
    }

    private void Add()
    {
        if (item.Level == item.Stat.Length)
            return;
        Player.Instance.GetComponent<ZCAttributeController>().UpgradeStat(item.Stat[item.Level]);
        item.Level++;
        UpdateItemInfo();
    }


}
