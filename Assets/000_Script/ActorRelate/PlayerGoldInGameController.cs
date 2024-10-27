using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGoldInGameController : MonoBehaviour
{
    int gold = 0;

    public int Gold { get => gold; set => gold = value; }

    private static PlayerGoldInGameController instance;
    public static PlayerGoldInGameController Instance { get { return instance; } private set { } }
    private void Awake()
    {
        if(instance==null)
            instance = this;
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        Player.Instance.GetComponent<ActorAtributeController>().onScoreChanged.AddListener(UpdateGold);
    }
    private void UpdateGold()
    {
        gold += Random.Range(1, 6);
    }
    public void TriggerAdMultiplyer(int multiplyer)
    {
        gold *= multiplyer;
    }
    public void OnEndCurrentLevel()
    {
        DataPersistenceManager.Instance.GameData.gold += gold;
        gold = 0;
    }
   

}
