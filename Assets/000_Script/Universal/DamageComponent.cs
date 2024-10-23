using System.Collections;
using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    protected ActorAttacker initiator;
    private bool isDestroyedAfterCollide = true;
    public bool IsDestroyedAfterCollide { get => isDestroyedAfterCollide; set => isDestroyedAfterCollide = value; }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IAttacker>() == initiator)
            return;

        if (other.CompareTag("AttackCircle") || other.CompareTag(gameObject.tag))
            return;
        LifeComponent lifeComponent = other.GetComponent<LifeComponent>();

        
        if (lifeComponent != null)
        {
            bool check = lifeComponent.DamageHealth(initiator.GetComponent<ActorInformationController>().GetName());
            if(check)
                initiator.EventIfKillSomeone();
            if (gameObject.CompareTag("Zombie"))
                return;
            SelfDestroyAfterCollide(isDestroyedAfterCollide);
        }
    }

    public void InitIAttacker(ActorAttacker master)
    {
        initiator = master;
    }

    protected virtual void SelfDestroyAfterCollide(bool isCanSelfDesttroy)
    {
        if(isCanSelfDesttroy)
            Destroy(gameObject);
        return;
    }
}