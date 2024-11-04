using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class DataPersistenceManager : MonoBehaviour
{
    [SerializeField] private string fileName;

    #region EssentialForNewGame
    [Header("NEW GAME CONFIG")]
    [SerializeField] private Weapon firstWeaponOfPlayer;
    [SerializeField] private Skin[] firstSkinOfPlayer;
    [SerializeField] private ShopItemWeapon[] shopItemWeaponFirst;
    [SerializeField] private ShopItemSkin[] shopItemSkinFirst;
    [SerializeField] private ZCStatItem[] zCStatItemFirst;
    #endregion
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObject;
    private FileDataHandler fileDataHandler;
    private static DataPersistenceManager instance;

    public static DataPersistenceManager Instance { get { return instance; } private set { } }
    public GameData GameData { get => gameData; private set{ } }

    #region[Database]
    public Dictionary<string, Weapon> WeaponDatabase { get => weaponDatabase; set => weaponDatabase = value; }
    public Dictionary<string, Skin> SkinDatabase { get => skinDatabase; set => skinDatabase = value; }
    public Dictionary<string, SetSkin> SetSkinDatabase { get => setSkinDatabase; set => setSkinDatabase = value; }

    private Dictionary<string, Weapon> weaponDatabase = new Dictionary<string, Weapon>();
    private Dictionary<string, Skin> skinDatabase = new Dictionary<string, Skin>();
    private Dictionary<string,SetSkin> setSkinDatabase = new Dictionary<string,SetSkin>();
    #endregion

    public UnityEvent OnGoldChange;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        LoadWeaponDatabase();
        LoadSkinDatabase();
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.dataPersistenceObject = FindAllDataPersistence();
        LoadGame();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneUnloaded -= OnSceneUnLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("SceneLoaded Called");
        this.dataPersistenceObject = FindAllDataPersistence();
        LoadGame();
    }
    public void OnSceneUnLoaded(Scene scene)
    {
        Debug.Log("SceneLoaded Called");
        SaveGame();
    }
    #region LoadDatabase
    private void LoadWeaponDatabase()
    {
        foreach (var shopItem in shopItemWeaponFirst)
        {
            if (!weaponDatabase.ContainsKey(shopItem.IdWeapon))
            {
                weaponDatabase.Add(shopItem.Weapon.IdWeapon, shopItem.Weapon);
            }
        }
        Debug.Log("Weapon database loaded with " + weaponDatabase.Count + " items.");
    }
    private void LoadSkinDatabase()
    {
        foreach (var shopItem in shopItemSkinFirst)
        {
            foreach (var skin in shopItem.SkinToAttach)
            {
                if (!skinDatabase.ContainsKey(skin.SkinId))
                {
                    skinDatabase.Add(skin.SkinId, skin);
                }
            }

            foreach (var setSkin in shopItem.SetSkinToAttach)
            {
                if (!setSkinDatabase.ContainsKey(setSkin.SetID))
                {
                    setSkinDatabase.Add(setSkin.SetID, setSkin);
                }
            }
        }
        foreach(var skin in firstSkinOfPlayer)
        {
            skinDatabase.Add(skin.SkinId.ToString(), skin);
        }
        Debug.Log("Skin database loaded with " + skinDatabase.Count + " items.");
    }
    #endregion
    public bool AccessGold(int amount)
    {
        if (gameData.gold + amount < 0)
        {
            return false;
        }
        else
        {
            gameData.gold += amount;
            OnGoldChange?.Invoke();
            return true;
        }
    }
    public void NewGame()
    {
        this.gameData = new GameData();
        string startingWeaponId = firstWeaponOfPlayer.IdWeapon;
        List<string> startingSkinIds = firstSkinOfPlayer.Select(skin => skin.SkinId).ToList();
        gameData.playerData.InitializePlayerData(startingWeaponId, startingSkinIds.ToArray(),false);
        gameData.levelData = new LevelData();
        for (int i = 0; i < shopItemWeaponFirst.Length; i++)
        {
            WeaponShopItemData weapon = new WeaponShopItemData();

            if (i == 0)
            {
                weapon.InitializeWeaponData(shopItemWeaponFirst[i].IdWeapon, true,2, new List<int> { 0, 1 });
                shopItemWeaponFirst[i].IsPurchased = true;
            }
            else 
            { 
                weapon.InitializeWeaponData(shopItemWeaponFirst[i].IdWeapon, false,0, new List<int> { 0, 1 });
                shopItemWeaponFirst[i].IsPurchased = false;
            }
            gameData.weaponDatas.Add(weapon);
        }
        for(int i = 0;i < shopItemSkinFirst.Length;i++)
        {
            for(int j = 0; j < shopItemSkinFirst[i].SkinToAttach.Length;j++)
            {
                SkinShopItemData  skin = new SkinShopItemData();
                skin.InitializeSkinData(shopItemSkinFirst[i].SkinToAttach[j].SkinId, false,false,false);
                shopItemSkinFirst[i].SkinToAttach[j].IsUnlock = false;
                gameData.skinDatas.Add(skin);

            }
            for (int j = 0; j < shopItemSkinFirst[i].SetSkinToAttach.Length; j++)
            {
                SkinShopItemData skin = new SkinShopItemData();
                skin.InitializeSkinData(shopItemSkinFirst[i].SetSkinToAttach[j].SetID, false,false,false);
                shopItemSkinFirst[i].SetSkinToAttach[j].IsUnlock = false;
                gameData.skinDatas.Add(skin);
            }
        }
        foreach (var item in zCStatItemFirst)
        {
            item.Level = 0;
            StatItemData itemData = new StatItemData();
            StatData itemData2 = new StatData();
            itemData.type = item.Type;
            itemData.level = item.Level + 1;
            itemData2.type = item.Type;
            itemData2.statNumber =  item.Stat[item.Level].HowMuchUpgrade;
            gameData.statItemDatas.Add(itemData);
            gameData.statDatas.Add(itemData2);

        }
    }
    public void LoadGame()
    {
        //
        this.gameData = fileDataHandler.Load();
        if(this.gameData == null)
        {
            Debug.Log("No data was found. Init data to defaults");
            NewGame();
        }
        foreach(IDataPersistence dataPersistence in dataPersistenceObject)
        {
            dataPersistence.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
        foreach(IDataPersistence dataPersistence in dataPersistenceObject)
        {
            dataPersistence.SaveData(ref gameData);
        }
        fileDataHandler.Save(gameData);
    }
   
    private void OnApplicationQuit()
    {
        Debug.Log("Save");
        Debug.Log(dataPersistenceObject.Count);

        SaveGame();
    }
    private List<IDataPersistence> FindAllDataPersistence()
    {
        IEnumerable<IDataPersistence> dataPersistences = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();
        return new List<IDataPersistence> (dataPersistences);
    }

}
