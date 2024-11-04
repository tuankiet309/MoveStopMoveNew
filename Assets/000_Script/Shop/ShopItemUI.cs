using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour,IDataPersistence
{
    [SerializeField] TextMeshProUGUI weaponName;
    [SerializeField] TextMeshProUGUI attributeBuff;
    [SerializeField] GameObject Varients;
    [SerializeField] Button varientGameObject;
    [SerializeField] GameObject displayWeaponHolder;

    [SerializeField] TextMeshProUGUI alertText;
    [SerializeField] Button equipButton;
    [SerializeField] Button unlockButton;
    [SerializeField] RectTransform ButtonToBuyHolder;
    [SerializeField] Button moneyButton;
    [SerializeField] Button leftButton;
    [SerializeField] Button rightButton;
    [SerializeField] ShopItemWeapon[] weaponItem;

    [SerializeField] Button purchaseBtn;
    [SerializeField] Button watchAdBtn;

    [SerializeField] Button CustomVarient;
    [SerializeField] RectTransform ColorPanel;
    [SerializeField] Button MaterialPref;

    private GameObject displayedWeapon;
    GameObject customVarient;
    private List<Button> variantButtons = new List<Button>();
    private Weapon weaponToChange;
    private int weaponIndex = 0;
    private int currentIndexOfTheSkin = 0;

    private void Awake()
    {

    }
    private void Start()
    {
        LoadInfomationOfCurrentWeapon();
        equipButton.onClick.AddListener(AttachWeapon);
        leftButton.onClick.AddListener(MoveLeft);
        rightButton.onClick.AddListener(MoveRight);
        purchaseBtn.onClick.AddListener(UnlockWeapon);
        watchAdBtn.onClick.AddListener(UnlockWeaponByWatchingAds);
    }


    private void MoveRight()
    {
        if (weaponIndex < weaponItem.Length - 1)
        {
            weaponIndex++;

            LoadInfomationOfCurrentWeapon();
        }
    }

    private void MoveLeft()
    {
        if (weaponIndex != 0)
        {
            weaponIndex--;
            LoadInfomationOfCurrentWeapon();
        }
    }

    private void LoadInfomationOfCurrentWeapon()
    {
        ColorPanel.gameObject.SetActive(false);
        if (displayedWeapon != null)
        {
            Destroy(displayedWeapon);
        }
        if (weaponItem[weaponIndex].IsPurchased == true)
        {
            weaponName.text = weaponItem[weaponIndex].WeaponName;
            attributeBuff.text = "+" + weaponItem[weaponIndex].Weapon.BuffMultiplyer.ToString() + " "+ weaponItem[weaponIndex].Weapon.Buff.ToString();
            alertText.gameObject.SetActive(false);
            currentIndexOfTheSkin = weaponItem[weaponIndex].Weapon.CurrentIndexOfTheSkin;
            displayedWeapon = Instantiate(weaponItem[weaponIndex].Weapon.PossibleSkinForThisWeapon[1].Skin, displayWeaponHolder.transform);
            InitializeVariants();
            Varients.gameObject.SetActive(true);
            weaponToChange = weaponItem[weaponIndex].Weapon;
            ButtonToBuyHolder.gameObject.SetActive(false);
            moneyButton.gameObject.SetActive(false);
            equipButton.gameObject.SetActive(true);
            unlockButton.gameObject.SetActive(false);
        }
        else if (weaponIndex >= 1)
        {
            if (weaponItem[weaponIndex].IsPurchased == false && weaponItem[weaponIndex - 1].IsPurchased == true)
            {
                weaponName.text = weaponItem[weaponIndex].WeaponName;
                attributeBuff.text = "+" + weaponItem[weaponIndex].Weapon.BuffMultiplyer.ToString() + " " + weaponItem[weaponIndex].Weapon.Buff.ToString();
                displayedWeapon = Instantiate(weaponItem[weaponIndex].Weapon.PossibleSkinForThisWeapon[1].Skin, displayWeaponHolder.transform);
                Varients.gameObject.SetActive(false);
                alertText.text = "Locked";
                moneyButton.gameObject.SetActive(false);
                alertText.gameObject.SetActive(true);
                ButtonToBuyHolder.gameObject.SetActive(true);
                watchAdBtn.transform.GetComponentInChildren<TextMeshProUGUI>().text = weaponItem[weaponIndex].TimeWatchAdsToPurchase.ToString() + "/2";
                purchaseBtn.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = weaponItem[weaponIndex].GoldCost.ToString();
                equipButton.gameObject.SetActive(false);
                unlockButton.gameObject.SetActive(false);
                ColorPanel.gameObject.SetActive(false);

            }
            else if (weaponItem[weaponIndex].IsPurchased == false && weaponItem[weaponIndex - 1].IsPurchased == false)
            {
                weaponName.text = weaponItem[weaponIndex].WeaponName;
                attributeBuff.text = "+" + weaponItem[weaponIndex].Weapon.BuffMultiplyer.ToString() + " " + weaponItem[weaponIndex].Weapon.Buff.ToString();
                alertText.text = "Unlock " + weaponItem[weaponIndex - 1].WeaponName + " first";
                displayedWeapon = Instantiate(weaponItem[weaponIndex].Weapon.PossibleSkinForThisWeapon[1].Skin, displayWeaponHolder.transform);
                Varients.gameObject.SetActive(false);
                moneyButton.gameObject.SetActive(true);
                moneyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = weaponItem[weaponIndex].GoldCost.ToString();
                alertText.gameObject.SetActive(true);
                ButtonToBuyHolder.gameObject.SetActive(false);
                equipButton.gameObject.SetActive(false);
                unlockButton.gameObject.SetActive(false);
                ColorPanel.gameObject.SetActive(false);
            }
        }
    }

    private void InitializeVariants()
    {
        foreach (Button variantButton in variantButtons)
        {
            Destroy(variantButton.gameObject);
            Destroy(customVarient.gameObject);
            CustomVarient.onClick.RemoveAllListeners();
        }
        variantButtons.Clear();

        int index = 0;
        foreach (var skinVariant in weaponItem[weaponIndex].Weapon.PossibleSkinForThisWeapon)
        {
            if (index == 0)
            {
                Transform itemDisplayer = CustomVarient.transform.GetChild(0);
                customVarient = Instantiate(skinVariant.Skin, itemDisplayer);
                customVarient.transform.localPosition = Vector3.zero;
                CustomVarient.onClick.AddListener(OpenColorPanel);
                CustomVarient.onClick.AddListener(() => ChangeWeaponToThis(0, false));
                CustomVarient.onClick.AddListener(SoundManager.Instance.PlayClickSound);
                
            }
            else
            {
                Button variantInstance = Instantiate(varientGameObject, Varients.transform);
                variantButtons.Add(variantInstance);
                Transform itemDisplay = variantInstance.transform.GetChild(0);
                Transform lockIcon = variantInstance.transform.GetChild(1);
                if (itemDisplay != null && lockIcon != null)
                {
                    bool IsLock = skinVariant.IsLocked;
                    lockIcon.gameObject.SetActive(IsLock);
                    GameObject skinDisplayInstance = Instantiate(skinVariant.Skin, itemDisplay);
                    skinDisplayInstance.transform.localPosition = Vector3.zero;

                    int currentIndex = index;
                    variantInstance.onClick.AddListener(() => ChangeWeaponToThis(currentIndex, IsLock));
                    variantInstance.onClick.AddListener(SoundManager.Instance.PlayClickSound);
                }
                else
                {
                    Debug.LogWarning("ItemDisplay not found inside the varientGameObject prefab!");
                }
                
            }
            index++;
        }
    }

    private void BuyThisSkin(int currentIndex,bool IsLock)
    {
        if (DataPersistenceManager.Instance.AccessGold(-weaponItem[weaponIndex].Weapon.PossibleSkinForThisWeapon[currentIndex].Gold))
        {
            weaponItem[weaponIndex].Weapon.PossibleSkinForThisWeapon[currentIndex].IsLocked = false;
            LoadInfomationOfCurrentWeapon();
            ChangeWeaponToThis(currentIndex, false);
            equipButton.onClick.Invoke();
        }

    }
    private void AttachWeapon()
    {
        WeaponComponent weaponComponent = Player.Instance.GetComponent<WeaponComponent>();
        if (weaponComponent != null)
        {
            weaponComponent.AssignWeapon(weaponToChange);
            weaponComponent.AssignWeapon(weaponToChange);
        }
    }
    private void ChangeWeaponToThis(int skinIndex, bool isLock)
    {
        MeshRenderer displayedWeaponRenderer = displayedWeapon.GetComponent<MeshRenderer>();
        MeshRenderer skinVariantRenderer = weaponItem[weaponIndex].Weapon.PossibleSkinForThisWeapon[skinIndex].Skin.GetComponent<MeshRenderer>();

        Material[] clonedMaterials = new Material[skinVariantRenderer.sharedMaterials.Length];
        for (int i = 0; i < clonedMaterials.Length; i++)
        {
            clonedMaterials[i] = new Material(skinVariantRenderer.sharedMaterials[i]); 
        }

        if (displayedWeaponRenderer != null)
        {
            displayedWeaponRenderer.sharedMaterials = clonedMaterials;
        }

        if (isLock)
        {
            equipButton.gameObject.SetActive(false);
            unlockButton.gameObject.SetActive(true);
            unlockButton.onClick.RemoveAllListeners();
            unlockButton.onClick.AddListener(()=>BuyThisSkin(skinIndex,isLock));
        }
        else
        {
            equipButton.gameObject.SetActive(true);
            unlockButton.gameObject.SetActive(false);
            weaponToChange = weaponItem[weaponIndex].Weapon;
            weaponItem[weaponIndex].Weapon.CurrentIndexOfTheSkin = skinIndex;
        }

        if (skinIndex != 0)
        {
            ColorPanel.gameObject.SetActive(false);
            ClearColorPanel();  
        }
    }
    private void OpenColorPanel()
    {
        unlockButton.gameObject.SetActive(false);
        equipButton.gameObject.SetActive(true);

        MeshRenderer displayedWeaponRenderer = displayedWeapon.GetComponent<MeshRenderer>();
        MeshRenderer customVarientRenderer = customVarient.GetComponent<MeshRenderer>();

        Material[] clonedMaterials = new Material[customVarientRenderer.sharedMaterials.Length];
        for (int i = 0; i < clonedMaterials.Length; i++)
        {
            clonedMaterials[i] = new Material(customVarientRenderer.sharedMaterials[i]);
        }

        if (displayedWeaponRenderer != null)
        {
            displayedWeaponRenderer.sharedMaterials = customVarientRenderer.sharedMaterials;
        }

        ColorPanelController colorController = ColorPanel.GetComponent<ColorPanelController>();
        colorController.InitializeMaterialPickers(displayedWeapon, customVarient);
        ColorPanel.gameObject.SetActive(true);
    }
    private void ClearColorPanel()
    {
        ColorPanelController colorController = ColorPanel.GetComponent<ColorPanelController>();

        foreach (Button btn in colorController.GetComponentsInChildren<Button>())
        {
            btn.onClick.RemoveAllListeners();
        }

        ColorPanel.gameObject.SetActive(false); 
    }
    private void UnlockWeapon()
    {
        if (DataPersistenceManager.Instance.AccessGold(-weaponItem[weaponIndex].GoldCost))
        {
            weaponItem[weaponIndex].IsPurchased = true;
            LoadInfomationOfCurrentWeapon();
        }
    }
    private void UnlockWeaponByWatchingAds()
    {
        weaponItem[weaponIndex].TimeWatchAdsToPurchase++;
        if (weaponItem[weaponIndex].TimeWatchAdsToPurchase >= 2)
        {
            weaponItem[weaponIndex].IsPurchased = true;
        }
        LoadInfomationOfCurrentWeapon();
    }
    public void LoadData(GameData gameData)
    {
        foreach (ShopItemWeapon shopItemWeapon in weaponItem)
        {
            WeaponShopItemData matchingWeaponData = gameData.weaponDatas.Find(weaponData => weaponData.id == shopItemWeapon.IdWeapon);

            if (matchingWeaponData != null)
            {
                shopItemWeapon.IsPurchased = matchingWeaponData.isPurchased;
                shopItemWeapon.TimeWatchAdsToPurchase = matchingWeaponData.timeToWatchAdToPuchase;

                foreach (var skin in shopItemWeapon.Weapon.PossibleSkinForThisWeapon)
                {
                    skin.IsLocked = true;
                }

                foreach (int skinIndex in matchingWeaponData.skinArePurchased)
                {
                    if (skinIndex < shopItemWeapon.Weapon.PossibleSkinForThisWeapon.Length)
                    {
                        shopItemWeapon.Weapon.PossibleSkinForThisWeapon[skinIndex].IsLocked = false;

                    }
                }
            }
        }
    }

    public void SaveData(ref GameData gameData)
    {
        foreach (ShopItemWeapon shopItemWeapon in weaponItem)
        {
            WeaponShopItemData matchingWeaponData = gameData.weaponDatas.Find(weaponData => weaponData.id == shopItemWeapon.IdWeapon);

            if (matchingWeaponData != null)
            {
                matchingWeaponData.isPurchased = shopItemWeapon.IsPurchased;
                matchingWeaponData.timeToWatchAdToPuchase = shopItemWeapon.TimeWatchAdsToPurchase;
                matchingWeaponData.skinArePurchased.Clear(); 

                for (int index = 0; index < shopItemWeapon.Weapon.PossibleSkinForThisWeapon.Length; index++)
                {
                    if (!shopItemWeapon.Weapon.PossibleSkinForThisWeapon[index].IsLocked)
                    {
                        matchingWeaponData.skinArePurchased.Add(index);
                    }
                }
            }
            else
            {
                WeaponShopItemData newWeaponData = new WeaponShopItemData();
                newWeaponData.InitializeWeaponData(shopItemWeapon.IdWeapon, shopItemWeapon.IsPurchased,shopItemWeapon.TimeWatchAdsToPurchase, new List<int>());

                for (int index = 0; index < shopItemWeapon.Weapon.PossibleSkinForThisWeapon.Length; index++)
                {
                    if (!shopItemWeapon.Weapon.PossibleSkinForThisWeapon[index].IsLocked)
                    {
                        newWeaponData.skinArePurchased.Add(index);
                    }
                }
                gameData.weaponDatas.Add(newWeaponData);
            }
        }
    }
}