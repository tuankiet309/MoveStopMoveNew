using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SetSkin", menuName = "Shop/SetSkin", order = 3)]
public class SetSkin : ScriptableObject
{
    [SerializeField] private string setID;
    [SerializeField] private Skin[] eachSkinOfSet;
    [SerializeField] Image imageToShow;
    [SerializeField] private bool isUnlock = false;
    [SerializeField] private int gold = 100;
    private bool isEquiped = false;
    public Skin[] SkinOfSet { get => eachSkinOfSet; set => eachSkinOfSet = value; }
    public Image ImageToShow { get => imageToShow; set => imageToShow = value; }
    public bool IsUnlock { get => isUnlock; set => isUnlock = value; }
    public bool IsEquiped { get => isEquiped; set => isEquiped = value; }
    public int Gold { get => gold; set => gold = value; }
    public string SetID { get => setID; set => setID = value; }

    [ContextMenu("GenerateIDForSet")]
    private void GenerateGUID()
    {
        setID = System.Guid.NewGuid().ToString();
    }
}
