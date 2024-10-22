using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZCAttributeController : ActorAtributeController
{
    
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
        playerVisualize.localScale += new Vector3(bodyScalerIncreaser, bodyScalerIncreaser, bodyScalerIncreaser);
        circle.UpdateCircleRadius(CONSTANT_VALUE.CIRCLE_RADIUS_INCREASER);
        if (visualizeCircle != null)
            visualizeCircle.sizeDelta = new Vector2(circle.CircleRadius * 2, circle.CircleRadius * 2);
        onPlayerUpgraded?.Invoke();
        scoreMilestone = 10000;
    }
    

}
