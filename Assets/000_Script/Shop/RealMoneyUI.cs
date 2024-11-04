using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RealMoneyUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] Button closeButton;
    [SerializeField] Button GoldPurchase;
    [SerializeField] Button NoAdPurchase;
    [SerializeField] Button SkinUnlockPurchase;
    [SerializeField] ShopSkinUI shop;
    private void Start()
    {
        closeButton.onClick.AddListener(CloseSelf);
        GoldPurchase.onClick.AddListener(GoldPurchased);
        SkinUnlockPurchase.onClick.AddListener(SkinUnlocked);
    }
    private void CloseSelf()
    {
        gameObject.SetActive(false);
    }
    private void GoldPurchased()
    {
        DataPersistenceManager.Instance.AccessGold(25000);
    }
    private void NoAdPurchased()
    {
        
    }
    private void SkinUnlocked()
    {
        foreach(var skin in DataPersistenceManager.Instance.SkinDatabase.Values)
        {
            skin.IsUnlock = true;
        }
        foreach(var skin in DataPersistenceManager.Instance.SetSkinDatabase.Values)
        {
            skin.IsUnlock = true;
        }
        shop.RefreshSkinUI();
    }
}
