using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static Enum;

public class ShopSkinUI : MonoBehaviour
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
    [SerializeField] RectTransform IfNotBuyYet;
    [SerializeField] Button buyButton;
    [SerializeField] Button buyOneTimeButton;
    [Space]
    [SerializeField] Button CloseButton;

    List<ButtonAndType> buttonList = new List<ButtonAndType>();
    private SkinComponent skinComp;
    private Skin[] currentTempSkin = null;

    private Dictionary<Enum.SkinType, Skin> currentlyEquippedSkins = new Dictionary<Enum.SkinType, Skin>();

    private void Awake()
    {
        CreateButton();
    }

    private void Start()
    {
        skinComp = Player.Instance.GetComponent<SkinComponent>();
        hatButton.onClick.AddListener(() => ShowHolder(hatHolder, hatButton));
        pantButton.onClick.AddListener(() => ShowHolder(pantHolder, pantButton));
        leftHandButton.onClick.AddListener(() => ShowHolder(leftHandHolder, leftHandButton));
        fullSetButton.onClick.AddListener(() => ShowHolder(fullSetHolder, fullSetButton));
        CloseButton.onClick.AddListener(() => skinComp.RevertSkin(true));
        ShowHolder(hatHolder, hatButton);
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

    void CreateButton()
    {
        AssignButton(0, hatItems, hatHolder);
        AssignButton(0, leftHandItems, leftHandHolder);
        AssignButton(0, pantItems, pantHolder);
        AssignSetButton(0);

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
            button1.onClick.AddListener(() => EventForChoseSkinButton(capturedIndex, shopItemSkin.SkinType, item.IsUnlock, button1));
            buttonList.Add(new ButtonAndType(button1,shopItemSkin.SkinType));
            index++;
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
            button1.onClick.AddListener(() => EventForChosenSetButton(capturedIndex, item.IsUnlock, button1));
            buttonList.Add(new ButtonAndType(button1, Enum.SkinType.Set));
            index++;
        }
    }

    void UpdateEquipButtonTextAndState(Enum.SkinType skinType, Button equippedButton)
    {
        foreach (var button in buttonList)
        {
            if (button.skinType == skinType || skinType == SkinType.Set || button.skinType == SkinType.Set) 
            {
                button.button.transform.GetChild(2).gameObject.SetActive(false);
            }
        }
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
                skinComp.AssignTempoSkin(currentTempSkin);

                equipButton.gameObject.SetActive(true);
                equipButton.onClick.RemoveAllListeners();
                equipButton.onClick.AddListener(() => EquipThisStuff(skinType, index, thisButton));
                IfNotBuyYet.gameObject.SetActive(false);
            }
            else
            {
                currentTempSkin = new Skin[] { selectedSkin };
                skinComp.AssignTempoSkin(currentTempSkin);

                equipButton.gameObject.SetActive(false);
                IfNotBuyYet.gameObject.SetActive(true);
            }
        }
    }


    void EventForChosenSetButton(int index, bool isUnlock, Button thisButton)
    {
        Skin[] selectedSkin = fullSetItems.SetSkinToAttach[index].SkinOfSet;
        if(selectedSkin != null)
        {
            if(isUnlock)
            {
                currentTempSkin = selectedSkin;
                skinComp.AssignTempoSkin(currentTempSkin);
                equipButton.gameObject.SetActive(true);
                equipButton.onClick.RemoveAllListeners();
                equipButton.onClick.AddListener(() => EquipThisStuff(Enum.SkinType.Set, index, thisButton));
                IfNotBuyYet.gameObject.SetActive(false);
            }
        }

    }

    void EquipThisStuff(Enum.SkinType skinType, int index, Button thisButton)
    {
        if (currentTempSkin != null && skinComp != null)
        {
            currentlyEquippedSkins[skinType] = currentTempSkin[0];
            skinComp.AssignNewSkin(currentTempSkin);
            UpdateEquipButtonTextAndState(skinType, thisButton);
            currentTempSkin = null;
            thisButton.transform.GetChild(2).gameObject.SetActive(true);
        }
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
