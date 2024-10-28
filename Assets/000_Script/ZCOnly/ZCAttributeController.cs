using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZCAttributeController : ActorAtributeController
{
    [SerializeField] ZCPower ZCPower;
    [SerializeField] List<ZCStatPlayer> stats;

    public List<ZCStatPlayer> Stats { get => stats; set => stats = value; }
    public ZCPower ZCPower1 { get => ZCPower; private set { } }

    public UnityEvent onChoseZCPower;
    public UnityEvent onUpgradeStat;

    int checkForUpgrade = 1;
    protected override void Start()
    {
        base.Start();
        scoreMilestone = 10;

    }
    protected override void UpdateScore()
    {
        score++;
        onScoreChanged?.Invoke();
        CheckForUpgrade();
    }

    protected override void CheckForUpgrade()
    {
        if (score >= scoreMilestone)
        {
            UpgradePlayer();
            ZCStatPlayer stat = stats.Find(stat => stat.Type == Enum.ZCUpgradeType.MaxWeapon);
            checkForUpgrade++;
            scoreMilestone += 10;
            if(checkForUpgrade == stat.HowMuchUpgrade)
            {
                scoreMilestone = 1000;
            }
        }
    }
    protected override void UpgradePlayer()
    {
        onPlayerUpgraded?.Invoke();
        scoreMilestone = 10000;
    }
    protected virtual void BiggerOnStart()
    {
        playerVisualize.localScale += new Vector3(bodyScalerIncreaser, bodyScalerIncreaser, bodyScalerIncreaser);
        circle.UpdateCircleRadius(CONSTANT_VALUE.CIRCLE_RADIUS_INCREASER);
        if (visualizeCircle != null)
            visualizeCircle.sizeDelta = new Vector2(circle.CircleRadius * 2, circle.CircleRadius * 2);
    }

    public void UpgradeStat(ZCStatPlayer newStat)
    {
        stats.Remove(stats.Find(stat => stat.Type == newStat.Type));
        stats.Add(newStat);
        onUpgradeStat?.Invoke();
    }

    public void SetZCPower(ZCPower zCPower)
    {
        this.ZCPower = zCPower;
        onChoseZCPower?.Invoke();
        if(ZCPower.PowerType == Enum.ZCPowerUp.Bigger)
        {
            this.BiggerOnStart();
        }

    }

    public override void LoadData(GameData gameData)
    {
        base.LoadData(gameData);
        foreach (var stat in stats) 
        {
           StatData data = gameData.statDatas.Find(statt => statt.type == stat.Type);
            if(data != null)
            {
                stat.HowMuchUpgrade = data.statNumber;
            }
            else
            {
                ZCStatPlayer statPlayer = new ZCStatPlayer();
                statPlayer.HowMuchUpgrade = data.statNumber;
                statPlayer.Type = data.type;
                statPlayer.Price = 0;
                stats.Add(statPlayer);
            }
        }
    }

    public override void SaveData(ref GameData gameData)
    {
        base.SaveData(ref gameData);
        List<StatData> data = new List<StatData>();
        foreach (var stat in stats) 
        {
            StatData statData = new StatData();
            statData.type = stat.Type;
            statData.statNumber = stat.HowMuchUpgrade;
            data.Add(statData);
        }
        if(data != null)
        {
            gameData.statDatas = data;
        }
    }


}


