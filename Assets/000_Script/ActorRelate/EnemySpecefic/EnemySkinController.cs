using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySkinController : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer skinToChanged;
    [SerializeField] SkinnedMeshRenderer pantToChanged;
    [SerializeField] Image imageToChangeColor;
    [SerializeField] TextMeshProUGUI nameToChangeColor;

    public SkinnedMeshRenderer SkinToChanged { get => skinToChanged; set => skinToChanged = value; }

    public void ChangeSkin(Material skinMat, Material pantMat)
    {
        skinToChanged.material = skinMat;
        pantToChanged.material = pantMat;
        imageToChangeColor.color = skinMat.color;
        nameToChangeColor.color= skinMat.color;
    }
}
