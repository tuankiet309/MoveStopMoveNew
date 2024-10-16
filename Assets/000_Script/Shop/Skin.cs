using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skin", menuName = "Skin", order = 1)]
public class Skin : ScriptableObject
{
    [SerializeField] GameObject skinToWear;
    [SerializeField] GameObject skinToShow;
    [SerializeField] Enum.SkinType skinType;
    [SerializeField] Vector3 skinPosOffsetOnWear;
    [SerializeField] Quaternion skinRotOffsetOnWear;
    [SerializeField] Enum.AttributeBuffs attributeBuffs;
    [SerializeField] int buffMultiplyer;

    public GameObject SkinToWear { get => skinToWear; set => skinToWear = value; }
    public Enum.SkinType SkinType { get => skinType; set => skinType = value; }
    public Vector3 SkinPosOffsetOnWear { get => skinPosOffsetOnWear; set => skinPosOffsetOnWear = value; }
    public Quaternion SkinRotOffsetOnWear { get => skinRotOffsetOnWear; set => skinRotOffsetOnWear = value; }
    public Enum.AttributeBuffs AttributeBuffs { get => attributeBuffs; set => attributeBuffs = value; }
    public int BuffMultiplyer { get => buffMultiplyer; set => buffMultiplyer = value; }
    public GameObject SkinToShow { get => skinToShow; set => skinToShow = value; }
}
