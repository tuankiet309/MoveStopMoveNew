using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallManager : MonoBehaviour
{
    /////////////////////Home//////////////////////
    private static HallManager instance;
    public static HallManager Instance {  get { return instance; } }

    public HallUI hallUI;

    public GameplayManager gamplayManager;

    private void Awake()
    {
        instance = this;
        GameplayManager.Instance = gamplayManager;
    }
    public void Init()
    {
        GameManager.Instance.hallManager = this;
        hallUI.Init();
    }

}
