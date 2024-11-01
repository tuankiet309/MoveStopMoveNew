using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Enum;

public class ShopSkinUI : MonoBehaviour,IDataPersistence
{
    [SerializeField] ShopItemSkin hatItems;
    [SerializeField] ShopItemSkin pantItems;
    [SerializeField] ShopItemSkin leftHandItems;
    [SerializeField] ShopItemSkin fullSetItems;
    [Space]
    [SerializeField] Button hatButton;
    [SerializeField] Button pantButton;
    [SerializeField] Button leftHandButton;
    [SerializeField] Button fullSetButton;
    [Space]
    [SerializeField] RectTransform hatHolder;
    [SerializeField] RectTransform pantHolder;
    [SerializeField] RectTransform leftHandHolder;
    [SerializeField] RectTransform fullSetHolder;
    [Space]
    [SerializeField] Button choseThisSkinButtonPrefab;
    [SerializeField] ScrollRect scrollRect;
    [Space]
    [SerializeField] Button equipButton;
    [SerializeField] Button unequipButton;
    [SerializeField] RectTransform IfNotBuyYet;
    [SerializeField] Button buyButton;
    [SerializeField] Button buyOneTimeButton;
    [Space]
    [SerializeField] Button CloseButton;

    List<ButtonAndType> buttonList = new List<ButtonAndType>();
    private SkinComponent skinComp;
    private Skin[] currentTempSkin = null;

    private Dictionary<Enum.SkinType, Skin> currentlyEquippedSkins = new Dictionary<Enum.SkinType, Skin>();

    private void OnDisable()
    {
        if (skinComp != null)
        {
            skinComp.RevertSkin(true);
        }
        GameData gameData = DataPersistenceManager.Instance.GameData;
        SaveData(ref gameData);
    }
    private bool buttonsInitialized = false; 
    private void Start()
    {
        hatButton.onClick.AddListener(() => ShowHolder(hatHolder, hatButton));
        pantButton.onClick.AddListener(() => ShowHolder(pantHolder, pantButton));
        leftHandButton.onClick.AddListener(() => ShowHolder(leftHandHolder, leftHandButton));
        fullSetButton.onClick.AddListener(() => ShowHolder(fullSetHolder, fullSetButton));
        CloseButton.onClick.AddListener(() => skinComp.RevertSkin(true));
    }
    bool firstEnable = true;
    private void OnEnable()
    {
        if (firstEnable)
        {
            firstEnable = false;
            return;
        }
        skinComp = Player.Instance.GetComponent<SkinComponent>();

        if (!buttonsInitialized)
        {
            CreateButton();
            buttonsInitialized = true;
        }
        ShowHolder(hatHolder, hatButton);
    }

    void CreateButton()
    {
        if (buttonsInitialized) return; 

        AssignButton(0, hatItems, hatHolder);
        AssignButton(0, leftHandItems, leftHandHolder);
        AssignButton(0, pantItems, pantHolder);
        AssignSetButton(0);
        buttonsInitialized = true; 
    }

    void ShowHolder(RectTransform holderToShow, Button button)
    {
        if (skinComp != null)
        {
            skinComp.RevertSkin();
        }
        hatHolder.gameObject.SetActive(false);
        pantHolder.gameObject.SetActive(false);
        leftHandHolder.gameObject.SetActive(false);
        fullSetHolder.gameObject.SetActive(false);
        holderToShow.gameObject.SetActive(true);
        holderToShow.GetChild(0).GetComponent<Button>().onClick.Invoke();
        scrollRect.content = holderToShow;
        SelectVisualize(button);
    }

    void SelectVisualize(Button button)
    {
        hatButton.image.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        pantButton.image.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        leftHandButton.image.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        fullSetButton.image.color = new Color(0.2f, 0.2f, 0.2f, 1f);
        hatButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.15f);
        pantButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.15f);
        leftHandButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.15f);
        fullSetButton.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.15f);
        button.image.color = new Color(1, 1, 1, 0f);
        button.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1f);
    }


    void AssignButton(int index, ShopItemSkin shopItemSkin, RectTransform holder)
    {
        foreach(var item in shopItemSkin.SkinToAttach)
        {
            Button button1 = Instantiate(choseThisSkinButtonPrefab, holder);
            GameObject Lock = button1.transform.GetChild(1).gameObject;
            Lock.SetActive(!item.IsUnlock);
            button1.transform.GetChild(2).gameObject.SetActive(false);
            GameObject objectToShow = Instantiate(item.SkinToShow, button1.transform.GetChild(0));
            objectToShow.transform.localPosition = shopItemSkin.PosOffsetOfThisType;
            int capturedIndex = index;
            button1.onClick.AddListener(() => skinComp.RevertSkin(true));
            button1.onClick.AddListener(() => EventForChoseSkinButton(capturedIndex, shopItemSkin.SkinType, item.IsUnlock, button1));
            button1.onClick.AddListener(SoundManager.Instance.PlayClickSound);
            buttonList.Add(new ButtonAndType(button1,shopItemSkin.SkinType));
            index++;

            if(skinComp.IsSkinCurrentlyEquipped(item.SkinType,item))
            {
                button1.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }
    void AssignSetButton(int index)
    {
        foreach (var item in fullSetItems.SetSkinToAttach)
        {
            Button button1 = Instantiate(choseThisSkinButtonPrefab, fullSetHolder);
            GameObject Lock = button1.transform.GetChild(1).gameObject;
            Lock.SetActive(!item.IsUnlock);
            button1.transform.GetChild(2).gameObject.SetActive(false);
            Image image = Instantiate(item.ImageToShow, button1.transform);
            int capturedIndex = index;
            button1.onClick.AddListener(() => skinComp.RevertSkin(true));
            button1.onClick.AddListener(() => EventForChosenSetButton(capturedIndex, item.IsUnlock, button1));
            button1.onClick.AddListener(SoundManager.Instance.PlayClickSound);
            buttonList.Add(new ButtonAndType(button1, Enum.SkinType.Set));
            index++;
            if (skinComp.IsSetCurrentlyEquipped( item.SkinOfSet))
            {
                button1.transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }

    private void UpdateEquipButtonTextAndState(Enum.SkinType skinType, Button equippedButton)
    {
        foreach (var button in buttonList)
        {
            if (button.skinType == skinType || skinType == SkinType.Set || button.skinType == SkinType.Set)
            {
                button.button.transform.GetChild(2).gameObject.SetActive(false); 
            }
        }
        IfNotBuyYet.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(true);
    }

    void EventForChoseSkinButton(int index, Enum.SkinType skinType, bool isUnlock, Button thisButton)
    {
        if (skinComp == null) return;

        Skin selectedSkin = null;
        if (skinType == Enum.SkinType.Hair)
        {
            selectedSkin = hatItems.SkinToAttach[index];
        }
        if (skinType == Enum.SkinType.LHand)
        {
            selectedSkin = leftHandItems.SkinToAttach[index];
        }
        if (skinType == Enum.SkinType.Pant)
        {
            selectedSkin = pantItems.SkinToAttach[index];
        }

        if (selectedSkin != null)
        {
            if (isUnlock)
            {
                currentTempSkin = new Skin[] { selectedSkin };
                skinComp.AssignTempoSkin(currentTempSkin, false);

                bool isCurrentlyEquipped = skinComp.IsSkinCurrentlyEquipped(skinType, selectedSkin);

                if (isCurrentlyEquipped)
                {
                    unequipButton.gameObject.SetActive(true);
                    equipButton.gameObject.SetActive(false);

                    unequipButton.onClick.RemoveAllListeners();
                    unequipButton.onClick.AddListener(() => UnequipSkin(skinType, thisButton));
                    unequipButton.onClick.AddListener(SoundManager.Instance.PlayClickSound);
                }
                else
                {
                    equipButton.gameObject.SetActive(true);
                    unequipButton.gameObject.SetActive(false);

                    equipButton.onClick.RemoveAllListeners();
                    equipButton.onClick.AddListener(() => EquipThisStuff(skinType, index, thisButton));
                    equipButton.onClick.AddListener(SoundManager.Instance.PlayClickSound);
                }
                IfNotBuyYet.gameObject.SetActive(false);
            }
            else
            {
                currentTempSkin = new Skin[] { selectedSkin };
                skinComp.AssignTempoSkin(currentTempSkin, false);

                equipButton.gameObject.SetActive(false);
                unequipButton.gameObject.SetActive(false);
                IfNotBuyYet.gameObject.SetActive(true);
                buyButton.onClick.RemoveAllListeners();
                buyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = selectedSkin.Gold.ToString();
                buyButton.onClick.AddListener(() => BuyThisSkin(selectedSkin,thisButton,index));
                buyButton.onClick.AddListener(SoundManager.Instance.PlayClickSound);


            }
            UnloadRing(thisButton);
        }
    }

    
    void EventForChosenSetButton(int index, bool isUnlock, Button thisButton)
    {
        Skin[] selectedSkinSet = fullSetItems.SetSkinToAttach[index].SkinOfSet;
        if (selectedSkinSet != null)
        {
            if (isUnlock)
            {
                currentTempSkin = selectedSkinSet;
                skinComp.AssignTempoSkin(currentTempSkin, true);
                bool isCurrentlyEquipped = skinComp.IsSetCurrentlyEquipped(selectedSkinSet);
                if (isCurrentlyEquipped)
                {
                    unequipButton.gameObject.SetActive(true);
                    equipButton.gameObject.SetActive(false);
                    unequipButton.onClick.RemoveAllListeners();
                    unequipButton.onClick.AddListener(() => UnequipSkin(Enum.SkinType.Set, thisButton));
                    unequipButton.onClick.AddListener(SoundManager.Instance.PlayClickSound);
                }
                else
                {
                    equipButton.gameObject.SetActive(true);
                    unequipButton.gameObject.SetActive(false);
                    equipButton.onClick.RemoveAllListeners();
                    equipButton.onClick.AddListener(() => EquipThisStuff(Enum.SkinType.Set, index, thisButton));
                    equipButton.onClick.AddListener(SoundManager.Instance.PlayClickSound);
                }
                thisButton.transform.GetChild(3).gameObject.SetActive(true);
                IfNotBuyYet.gameObject.SetActive(false);
            }
            else
            {
                currentTempSkin = selectedSkinSet;
                skinComp.AssignTempoSkin(currentTempSkin, true);
                equipButton.gameObject.SetActive(false);
                unequipButton.gameObject.SetActive(false);
                IfNotBuyYet.gameObject.SetActive(true);
                buyButton.onClick.RemoveAllListeners();
                buyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = fullSetItems.SetSkinToAttach[index].Gold.ToString();
                buyButton.onClick.AddListener(()=>BuyThisSet(fullSetItems.SetSkinToAttach[index],thisButton,index));
                buyButton.onClick.AddListener(SoundManager.Instance.PlayClickSound);
            }
            UnloadRing(thisButton);

        }
    }

    private void RefreshSkinUI()
    {
        foreach (Transform child in hatHolder) Destroy(child.gameObject);
        foreach (Transform child in leftHandHolder) Destroy(child.gameObject);
        foreach (Transform child in pantHolder) Destroy(child.gameObject);
        foreach (Transform child in fullSetHolder) Destroy(child.gameObject);

        buttonList.Clear(); 

        AssignButton(0, hatItems, hatHolder);
        AssignButton(0, leftHandItems, leftHandHolder);
        AssignButton(0, pantItems, pantHolder);
        AssignSetButton(0);
    }

    private void BuyThisSkin(Skin selectedSkin, Button theSkinButton, int index)
    {
        if (DataPersistenceManager.Instance.AccessGold(-selectedSkin.Gold))
        {
            selectedSkin.IsUnlock = true;
            theSkinButton.transform.GetChild(1).gameObject.SetActive(false);
            IfNotBuyYet.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(() => EquipThisStuff(selectedSkin.SkinType, index, theSkinButton));
            equipButton.onClick.Invoke();
        }
    }
    private void BuyThisSet(SetSkin setSkin, Button theSkinButton, int index)
    {
        if (DataPersistenceManager.Instance.AccessGold(-setSkin.Gold))
        {
            setSkin.IsUnlock = true;
            equipButton.gameObject.SetActive(true);
            theSkinButton.transform.GetChild(1).gameObject.SetActive(false);
            IfNotBuyYet.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(() => EquipThisStuff(Enum.SkinType.Set, index, theSkinButton));
            equipButton.onClick.Invoke();
        }
    }
    void UnloadRing(Button thisButton)
    {
        foreach(ButtonAndType bt in buttonList)
        {
            bt.button.transform.GetChild(3).gameObject.SetActive(false);
        }
        if(thisButton != null)
            thisButton.transform.GetChild(3).gameObject.SetActive(true);
    }
    void EquipThisStuff(Enum.SkinType skinType, int index, Button thisButton)
    {
        if (currentTempSkin != null && skinComp != null)
        {
            currentlyEquippedSkins[skinType] = currentTempSkin[0];
            skinComp.AssignNewSkin(currentTempSkin, skinType == Enum.SkinType.Set);
            UpdateEquipButtonTextAndState(skinType, thisButton);
            currentTempSkin = null;
            thisButton.transform.GetChild(2).gameObject.SetActive(true);
            equipButton.gameObject.SetActive(false);
            unequipButton.gameObject.SetActive(true);
            unequipButton.onClick.RemoveAllListeners();
            unequipButton.onClick.AddListener(() => UnequipSkin(skinType, thisButton));
            unequipButton.onClick.AddListener(SoundManager.Instance.PlayClickSound);

        }
    }
    public void UnequipSkin(Enum.SkinType skinType, Button thisButton)
    {
        skinComp.UnequipCurrentSkin(skinType);
        thisButton.transform.GetChild(2).gameObject.SetActive(false);
        thisButton.transform.GetChild(3).gameObject.SetActive(false);
        unequipButton.gameObject.SetActive(false);
        thisButton.interactable = true;
       
    }
    void ClearAllSkinComponents()
    {
        skinComp.ClearSkin(Enum.SkinType.Hair);
        skinComp.ClearSkin(Enum.SkinType.LHand);
        skinComp.ClearSkin(Enum.SkinType.Wing);
        skinComp.ClearSkin(Enum.SkinType.Tail);
        skinComp.ClearSkin(Enum.SkinType.Pant);
        skinComp.ClearSkin(Enum.SkinType.Body);
    }

    public void LoadData(GameData gameData)
    {
        foreach(Skin skin in hatItems.SkinToAttach)
        {
            SkinShopItemData skinData = gameData.skinDatas.Find(skinDt => skinDt.idOfSkin == skin.SkinId);
            skin.IsUnlock = skinData.isPurchased;
        }
        foreach (Skin skin in leftHandItems.SkinToAttach)
        {
            SkinShopItemData skinData = gameData.skinDatas.Find(skinDt => skinDt.idOfSkin == skin.SkinId);
            skin.IsUnlock = skinData.isPurchased;
        }
        foreach (Skin skin in pantItems.SkinToAttach)
        {
            SkinShopItemData skinData = gameData.skinDatas.Find(skinDt => skinDt.idOfSkin == skin.SkinId);
            skin.IsUnlock = skinData.isPurchased;
        }
        foreach (SetSkin skin in fullSetItems.SetSkinToAttach)
        {
            SkinShopItemData skinData = gameData.skinDatas.Find(skinDt => skinDt.idOfSkin == skin.SetID);
            skin.IsUnlock = skinData.isPurchased;
        }
    }

    public void SaveData(ref GameData gameData)
    {
        foreach (Skin skin in hatItems.SkinToAttach)
        {
            SkinShopItemData skinData = gameData.skinDatas.Find(skinDt => skinDt.idOfSkin == skin.SkinId);
            if (skinData != null)
            {
                skinData.isPurchased = skin.IsUnlock;
            }
            else
            {
                skinData.InitializeSkinData(skin.SkinId, false);
                gameData.skinDatas.Add(skinData);
            }
        }

        foreach (Skin skin in leftHandItems.SkinToAttach)
        {
            SkinShopItemData skinData = gameData.skinDatas.Find(skinDt => skinDt.idOfSkin == skin.SkinId);
            if (skinData != null)
            {
                skinData.isPurchased = skin.IsUnlock;
            }
            else
            {
                skinData.InitializeSkinData(skin.SkinId, false);
                gameData.skinDatas.Add(skinData);
            }
        }

        foreach (Skin skin in pantItems.SkinToAttach)
        {
            SkinShopItemData skinData = gameData.skinDatas.Find(skinDt => skinDt.idOfSkin == skin.SkinId);
            if (skinData != null)
            {
                skinData.isPurchased = skin.IsUnlock;
            }
            else
            {
                skinData.InitializeSkinData(skin.SkinId, false);
                gameData.skinDatas.Add(skinData);
            }
        }

        foreach (SetSkin skin in fullSetItems.SetSkinToAttach)
        {
            SkinShopItemData skinData = gameData.skinDatas.Find(skinDt => skinDt.idOfSkin == skin.SetID);
            if (skinData != null)
            {
                skinData.isPurchased = skin.IsUnlock;
            }
            else
            {
                skinData.InitializeSkinData(skin.SetID, false);
                gameData.skinDatas.Add(skinData);
            }
        }
    }
}
class ButtonAndType
{
    public Button button;
    public Enum.SkinType skinType;
    public ButtonAndType (Button button, Enum.SkinType skinType)
    {
        this.button = button;
        this.skinType = skinType;
    }
}
