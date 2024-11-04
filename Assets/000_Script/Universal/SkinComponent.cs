using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Enum;

public class SkinComponent : MonoBehaviour,IDataPersistence
{


    [Header("Essential")]
    [SerializeField] Transform hatHolder;
    [SerializeField] Transform LHandHolder;
    [SerializeField] Transform wingHolder;
    [SerializeField] Transform tailHolder;
    [SerializeField] SkinnedMeshRenderer pantToChange;
    [SerializeField] SkinnedMeshRenderer playerSkinToChange;
    [Space]
    [SerializeField] TextMeshProUGUI nameToChange;
    [SerializeField] Image imageToChange;

    [SerializeField] Skin[] defaultSkin;

    private bool isASet = false;
    private List<Skin> skinToChange = new List<Skin>();
    private List<Skin> previousSkins = new List<Skin>();
    private Material originalPantMaterial;
    private Material originalSkinMaterial;

    public UnityEvent<List<Skin>,List<Skin>> onWearNewSkin;

    private void Awake()
    {
        SaveOriginalMaterials();
    }
    private void Start()
    {
        previousSkins = skinToChange;
        GameManager.Instance.onStateChange.AddListener(CheckOnceTimeSkin);
    }
    private void OnDisable()
    {
        GameManager.Instance.onStateChange.RemoveListener(CheckOnceTimeSkin);
    }

    public void AssignNewSkin(Skin[] newSkin, bool isASet)
    {
        onWearNewSkin?.Invoke(previousSkins,skinToChange);
        skinToChange = newSkin.ToList();
        if (this.isASet || isASet)
        {
            ClearAllSkinComponents();
            previousSkins.Clear();
            previousSkins = skinToChange;
        }
        else 
            foreach (Skin skin in skinToChange)
            {
            Skin existingSkin = previousSkins.FirstOrDefault(s => s.SkinType == skin.SkinType);
            if (existingSkin != null)
            {
                previousSkins.Remove(existingSkin);
            }
            previousSkins.Add(skin);
            }
        this.isASet = isASet;
        WearSkin(skinToChange);
    }
    private void CheckOnceTimeSkin(GameState gameState, InGameState inGameState)
    {
        if ((gameState == GameState.Ingame && inGameState == InGameState.PVE) || (gameState == GameState.Begin))
        {
            List<Skin> skinsToRemove = new List<Skin>();
            foreach (Skin skin in skinToChange)
            {
                if (skin.IsUnlockedOnce == true)
                {
                    skin.IsUsedYet = true;
                    skin.IsEquiped = false;
                    skin.IsUnlockedOnce = false;
                    skin.IsUnlock = false;

                    skinsToRemove.Add(skin);
                }
            }

            foreach (Skin skin in skinsToRemove)
            {
                skinToChange.Remove(skin);
                previousSkins.Remove(skin);
            }
        }
    }
    public void AssignTempoSkin(Skin[] tempoSkin,bool isASet)
    {
        if (this.isASet || isASet)
            ClearAllSkinComponents();
        skinToChange = tempoSkin.ToList();
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
                pantToChange.sharedMaterial = skin.SkinToWear.GetComponent<MeshRenderer>().sharedMaterial;
                
            }
            if (skin.SkinType == Enum.SkinType.Body)
            {
                playerSkinToChange.sharedMaterial = skin.SkinToWear.GetComponent<MeshRenderer>().sharedMaterial;
                if (imageToChange != null && nameToChange != null)
                {
                    imageToChange.color = playerSkinToChange.sharedMaterial.color;
                    nameToChange.color = playerSkinToChange.sharedMaterial.color;
                }
            }
        }
    }

    private void SaveOriginalMaterials()
    {
        foreach (Skin skin in defaultSkin)
        {
            if (skin.SkinType == Enum.SkinType.Pant && originalPantMaterial == null)
            {
                originalPantMaterial = skin.SkinToWear.GetComponent<MeshRenderer>().sharedMaterial; 
            }
            if (skin.SkinType == Enum.SkinType.Body && originalSkinMaterial == null)
            {
                originalSkinMaterial = skin.SkinToWear.GetComponent<MeshRenderer>().sharedMaterial;
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
    public bool IsSkinCurrentlyEquipped(Enum.SkinType skinType, Skin skin)
    {
        Skin equippedSkin = previousSkins.FirstOrDefault(s => s.SkinType == skinType);
        return equippedSkin != null && equippedSkin == skin;
    }

    public bool IsSetCurrentlyEquipped(Skin[] skinSet)
    {
        foreach (Skin skin in skinSet)
        {
            Skin equippedSkin = previousSkins.FirstOrDefault(s => s.SkinType == skin.SkinType);
            if (equippedSkin == null || equippedSkin != skin)
            {
                return false; 
            }
        }
        return true;
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
    public void UnequipCurrentSkin(Enum.SkinType skinType)
    {
        if (skinType == Enum.SkinType.Set)
        {
            ClearAllSkinComponents();
            skinToChange = defaultSkin.ToList();
            previousSkins = skinToChange;
        }
        ClearSkin(skinType);
        skinToChange.RemoveAll(skin => skin.SkinType == skinType);
        previousSkins.RemoveAll(skin => skin.SkinType == skinType);
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

    public void LoadData(GameData gameData)
    {
        skinToChange.Clear();
        previousSkins.Clear();
        if(gameData.playerData.isASet)
        {
            skinToChange = DataPersistenceManager.Instance.SetSkinDatabase[gameData.playerData.playerCurrentWearingSkinID[0]].SkinOfSet.ToList();
        }
        else
        {
            foreach(string id in gameData.playerData.playerCurrentWearingSkinID)
            {
                skinToChange.Add(DataPersistenceManager.Instance.SkinDatabase[id]);
            }
        }
        isASet = gameData.playerData.isASet;
        AssignNewSkin(skinToChange.ToArray(), isASet);
        previousSkins = skinToChange;
    }

    public void SaveData(ref GameData gameData)
    {

        List<string> skinIDs = new List<string>();
        if (isASet)
        {
            foreach (SetSkin setSkin in DataPersistenceManager.Instance.SetSkinDatabase.Values)
            {
                if (setSkin.SkinOfSet.Contains<Skin>(previousSkins[0]))
                {
                    skinIDs.Add(setSkin.SetID);
                    gameData.playerData.playerCurrentWearingSkinID = skinIDs.ToArray();
                }
            }

        }
        else
        {
            foreach (Skin skin in previousSkins)
            {
                skinIDs.Add(skin.SkinId);
            }
            gameData.playerData.playerCurrentWearingSkinID = skinIDs.ToArray();
        }
        gameData.playerData.isASet = isASet;
    }
}