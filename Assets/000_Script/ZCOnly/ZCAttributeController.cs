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
        for (int i = 0; i < stats.Count; i++)
        {
            if (stats[i].Type == newStat.Type)
            {
                stats.RemoveAt(i);  
                break; 
            }
        }
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
}


