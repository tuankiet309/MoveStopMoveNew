using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SkinItem", menuName = "Shop/SkinItem", order = 2)]
public class ShopItemSkin : ScriptableObject
{
    [SerializeField] Skin[] skinToAttach;
    [SerializeField] SetSkin[] setSkinToAttach;
    [SerializeField] Enum.SkinType skinType;

    [SerializeField] Vector3 posOffsetOfThisType = Vector3.zero;

    int currentIndexOfSkin = 0;

    public Skin[] SkinToAttach { get => skinToAttach; set => skinToAttach = value; }
    public Enum.SkinType SkinType { get => skinType; set => skinType = value; }
    public SetSkin[] SetSkinToAttach { get => setSkinToAttach; set => setSkinToAttach = value; }
    public int CurrentIndexOfSkin { get => currentIndexOfSkin; set => currentIndexOfSkin = value; }
    public Vector3 PosOffsetOfThisType { get => posOffsetOfThisType; set => posOffsetOfThisType = value; }
}


