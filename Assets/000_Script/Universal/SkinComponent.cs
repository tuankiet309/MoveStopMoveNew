using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private List<Skin> skinToChange = new List<Skin>();

    private void Start()
    {
        skinToChange.Clear();
        skinToChange = skin.ToList();
        WearSkin();
    }
    public void AssignSkin(Skin[] skin )
    {
        skinToChange = skin.ToList();
    }

    private void WearSkin()
    {
        foreach (Skin skin in skinToChange) 
        {
            if(skin.SkinType == Enum.SkinType.Hair)
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
}
