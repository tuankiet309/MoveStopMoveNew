using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SkinComponent : MonoBehaviour
{
    [SerializeField] Transform hatHolder;
    [SerializeField] Transform LHandHolder;
    [SerializeField] Transform wingHolder;
    [SerializeField] Transform tailHolder;
    [SerializeField] GameObject pantToChange;
    [SerializeField] GameObject playerSkinToChange;

    [SerializeField] Skin[] skin;

    private bool isASet = false;
    private List<Skin> skinToChange = new List<Skin>();
    private List<Skin> previousSkins = new List<Skin>();


    private Material originalPantMaterial;
    private Material originalSkinMaterial;



    private void Start()
    {
        skinToChange.Clear();
        skinToChange = skin.ToList();
        SaveOriginalMaterials();
        WearSkin(skinToChange);
    }

    public void AssignNewSkin(Skin[] newsSkin, bool isASet)
    {
        skinToChange.Clear();
        skinToChange = newsSkin.ToList();
        previousSkins.Clear();
        previousSkins = skinToChange;

        this.isASet = isASet;
        ClearAllSkinComponents();
        WearSkin(skinToChange);   
    }

    public void AssignTempoSkin(Skin[] tempoSkin,bool isASet)
    {
        skinToChange = tempoSkin.ToList();
        this.isASet = isASet;
        ClearAllSkinComponents();
        WearSkin(skinToChange);
    }

    public void RevertSkin(bool revertMaterials = false)
    {
        for (int i = 0; i < skinToChange.Count; i++)
        {
            ClearSkin(skinToChange[i].SkinType);
        }
        skinToChange = previousSkins;
        WearSkin(skinToChange);
    }

    private void WearSkin(List<Skin> wearThisSkin)
    {
        foreach (Skin skin in wearThisSkin)
        {
            ClearSkin(skin.SkinType);

            if (skin.SkinType == Enum.SkinType.Hair)
            {
                GameObject gameObject = Instantiate(skin.SkinToWear, hatHolder);
                gameObject.transform.localPosition = skin.SkinPosOffsetOnWear;
                gameObject.transform.localRotation = skin.SkinRotOffsetOnWear;
            }
            if (skin.SkinType == Enum.SkinType.LHand)
            {
                GameObject gameObject = Instantiate(skin.SkinToWear, LHandHolder);
                gameObject.transform.localPosition = skin.SkinPosOffsetOnWear;
                gameObject.transform.localRotation = skin.SkinRotOffsetOnWear;
            }
            if (skin.SkinType == Enum.SkinType.Wing)
            {
                GameObject gameObject = Instantiate(skin.SkinToWear, wingHolder);
                gameObject.transform.localPosition = skin.SkinPosOffsetOnWear;
                gameObject.transform.localRotation = skin.SkinRotOffsetOnWear;
            }
            if (skin.SkinType == Enum.SkinType.Tail)
            {
                GameObject gameObject = Instantiate(skin.SkinToWear, tailHolder);
                gameObject.transform.localPosition = skin.SkinPosOffsetOnWear;
                gameObject.transform.localRotation = skin.SkinRotOffsetOnWear;
            }
            if (skin.SkinType == Enum.SkinType.Pant)
            {
                pantToChange.GetComponent<SkinnedMeshRenderer>().sharedMaterial = skin.SkinToWear.GetComponent<MeshRenderer>().sharedMaterial;
            }
            if (skin.SkinType == Enum.SkinType.Body)
            {
                playerSkinToChange.GetComponent<SkinnedMeshRenderer>().sharedMaterial = skin.SkinToWear.GetComponent<MeshRenderer>().sharedMaterial;
            }
        }
    }

    private void SaveOriginalMaterials()
    {
        for(int i = 0;i<skinToChange.Count;i++)
        {
            if(skinToChange[i].SkinType == Enum.SkinType.Pant)
            {
                originalPantMaterial = skinToChange[i].SkinToWear.GetComponent<MeshRenderer>().sharedMaterial;
            }
            if (skinToChange[i].SkinType == Enum.SkinType.Body)
            {
                originalSkinMaterial = skinToChange[i].SkinToWear.GetComponent<MeshRenderer>().sharedMaterial;
            }
        }
    }

    private void RevertOriginalMaterials()
    {
        if (originalPantMaterial != null && pantToChange != null)
        {
            pantToChange.GetComponent<SkinnedMeshRenderer>().sharedMaterial = originalPantMaterial;
        }
        if (originalSkinMaterial != null && playerSkinToChange != null)
        {
            playerSkinToChange.GetComponent<SkinnedMeshRenderer>().sharedMaterial = originalSkinMaterial;
        }
    }

    public void ClearSkin(Enum.SkinType typeToDelete)
    {
        if (typeToDelete == Enum.SkinType.Hair || typeToDelete == Enum.SkinType.Set)
        {
            foreach (Transform child in hatHolder)
            {
                Destroy(child.gameObject);
            }
        }

        if (typeToDelete == Enum.SkinType.LHand || typeToDelete == Enum.SkinType.Set)
        {
            foreach (Transform child in LHandHolder)
            {
                Destroy(child.gameObject);
            }
        }

        if (typeToDelete == Enum.SkinType.Wing || typeToDelete == Enum.SkinType.Set)
        {
            foreach (Transform child in wingHolder)
            {
                Destroy(child.gameObject);
            }
        }
        if (typeToDelete == Enum.SkinType.Tail || typeToDelete == Enum.SkinType.Set)
        {
            foreach (Transform child in tailHolder)
            {
                Destroy(child.gameObject);
            }
        }
        if (typeToDelete == Enum.SkinType.Pant )
        {
            pantToChange.GetComponent<SkinnedMeshRenderer>().sharedMaterial = originalPantMaterial;
        }
        if(typeToDelete == Enum.SkinType.Body)
        {
            playerSkinToChange.GetComponent<SkinnedMeshRenderer>().sharedMaterial = originalSkinMaterial;
        }
    }
    void ClearAllSkinComponents()
    {
        ClearSkin(Enum.SkinType.Hair);
        ClearSkin(Enum.SkinType.LHand);
        ClearSkin(Enum.SkinType.Wing);
        ClearSkin(Enum.SkinType.Tail);
        ClearSkin(Enum.SkinType.Pant);
        ClearSkin(Enum.SkinType.Body);
    }
}