using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZCManager : MonoBehaviour
{
    private static ZCManager instance;
    public static ZCManager Instance {  get { return instance; } set { instance = value; } }

    public ZombieSpawner Spawner;
    
    public void Init()
    {

    }
}
