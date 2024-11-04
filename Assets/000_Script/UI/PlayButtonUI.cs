using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayButtonUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI zone;
    void Start()
    {
        zone.text = "Zone: "+(DataPersistenceManager.Instance.GameData.levelData.currentPVELevel+1).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
