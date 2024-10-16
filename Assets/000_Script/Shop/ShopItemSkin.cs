using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkinItem", menuName = "Shop/SkinItem", order = 2)]
public class ShopItemSkin : ScriptableObject
{
    [SerializeField] Skin[] skinToAttach;
    [SerializeField] SetSkin[] setSkinToAttach;
    [SerializeField] int gold;
    [SerializeField] Enum.SkinType skinType;
    [SerializeField] bool isUnlock = false;
    [SerializeField] bool isUnlockedOnce = false;

    public Skin[] SkinToAttach { get => skinToAttach; set => skinToAttach = value; }
    public int Gold { get => gold; set => gold = value; }
    public Enum.SkinType SkinType { get => skinType; set => skinType = value; }
    public bool IsUnlock { get => isUnlock; set => isUnlock = value; }
    public bool IsUnlockedOnce { get => isUnlockedOnce; set => isUnlockedOnce = value; }
    public SetSkin[] SetSkinToAttach { get => setSkinToAttach; set => setSkinToAttach = value; }
}

[Serializable]
public class SetSkin
{
    [SerializeField] private Skin[] skinOfSet;

    public Skin[] SkinOfSet { get => skinOfSet; set => skinOfSet = value; }
}
