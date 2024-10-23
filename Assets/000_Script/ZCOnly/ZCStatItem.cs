using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ZCSTAT", menuName = "ZC/ZCSTAT", order = 2)]
public class ZCStatItem : ScriptableObject
{
    [SerializeField] private Enum.ZCUpgradeType type;
    [SerializeField] private Sprite stateImage;
    [SerializeField] private int level = 0;
    [SerializeField] private ZCStatPlayer[] stat;

    public Enum.ZCUpgradeType Type { get => type; set => type = value; }
    public int Level { get => level; set => level = value; }
    public ZCStatPlayer[] Stat { get => stat; set => stat = value; }
    public Sprite StateImage { get => stateImage; set => stateImage = value; }
}
[Serializable]
public class ZCStatPlayer
{
    [SerializeField] private Enum.ZCUpgradeType type;
    [SerializeField] private int howMuchUpgrade;
    [SerializeField] private int price;

    public Enum.ZCUpgradeType Type { get => type; set => type = value; }
    public int HowMuchUpgrade { get => howMuchUpgrade; set => howMuchUpgrade = value; }
    public int Price { get => price; set => price = value; }
}