using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZCAttributeController : ActorAtributeController
{
    [SerializeField] ZCPower ZCPower;
    [SerializeField] List<ZCStatPlayer> stats;

    public List<ZCStatPlayer> Stats { get => stats; set => stats = value; }
    public ZCPower ZCPower1 { get => ZCPower; set => ZCPower = value; }
    protected override void Start()
    {
        base.Start();

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
        }
    }
    protected override void UpgradePlayer()
    {
        onPlayerUpgraded?.Invoke();
        scoreMilestone = 10000;
    }
    
    public void UpgradeStat(ZCStatPlayer newStat)
    {
        foreach (var stat in stats)
        {
            if(stat.Type == newStat.Type)
            {
                stats.Remove(stat);
                stats.Add(newStat);
            }
        }
    }
}


