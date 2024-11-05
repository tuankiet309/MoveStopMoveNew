using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZCLifeComponent : LifeComponent
{
    [SerializeField] private ZCAttributeController attributeController;
    [SerializeField] private GameObject protectedCircle;

    private ZCStatPlayer statProtect;
    private int numberOfUndyingTime = 0;
    private bool isInvincibleNow;

    //public override void InitInitLifeComponent()
    //{
    //    base.Start();
    //    if (attributeController != null)
    //    {
    //        attributeController.onUpgradeStat.AddListener(OnUpgradeProtect);
    //        statProtect = attributeController.Stats.Find(x => x.Type == Enum.ZCUpgradeType.Protect); 
    //    }
    //    if (statProtect != null)
    //    {
    //        numberOfUndyingTime = statProtect.HowMuchUpgrade;
    //    }
    //    if(protectedCircle != null) 
    //        protectedCircle.SetActive(false);
    //}

    public override bool DamageHealth(string attackerName)
    {
        if (isInvincibleNow)
        {
            return false; 
        }

        health -= 1;

        if (numberOfUndyingTime > 0)
        {
            StartCoroutine(ActivateProtectedCircle());
            numberOfUndyingTime--;
            return false;
        }

        if (health <= 0)
        {
            onLifeEnds?.Invoke(attackerName);
            ParticleSpawner.Instance.PlayParticle(transform.position+Vector3.up, actorMeshRenderer.sharedMaterial);
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator ActivateProtectedCircle()
    {
        protectedCircle.SetActive(true);
        isInvincibleNow = true; 

        Renderer circleRenderer = protectedCircle.GetComponent<Renderer>();
        Material circleMaterial = circleRenderer.material;

        float blinkDuration = 1.0f;
        float blinkInterval = 0.1f;
        Color startColor = circleMaterial.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0.5f);

        float elapsedTime = 0f;
        while (elapsedTime < blinkDuration)
        {
            circleMaterial.color = Color.Lerp(startColor, endColor, Mathf.PingPong(elapsedTime / blinkInterval, 1));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        circleMaterial.color = startColor;

        yield return new WaitForSeconds(1f);

        isInvincibleNow = false; 
        protectedCircle.SetActive(false);
    }
    private void OnUpgradeProtect(ZCStatPlayer player)
    {
        if(player.Type == Enum.ZCUpgradeType.Protect)
        {
            numberOfUndyingTime = player.HowMuchUpgrade;
        }
    }
}