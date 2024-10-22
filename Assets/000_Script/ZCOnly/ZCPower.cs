using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ZCPOWER", menuName = "ZC/ZCPOWER", order =1)]
public class ZCPower : ScriptableObject
{
    [SerializeField] private string powerName;
    [SerializeField] private Sprite powerImage;
    [SerializeField] private Enum.ZCPowerUp powerType;

    public string PowerName { get => powerName; set => powerName = value; }
    public Sprite PowerImage { get => powerImage; set => powerImage = value; }
    public Enum.ZCPowerUp PowerType { get => powerType; set => powerType = value; }
}
