using System.Collections;
using UnityEngine;

public class DamageComponent : MonoBehaviour
{
    protected ActorAttacker initiator;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IAttacker>() == initiator)
            return;

        if (other.CompareTag("AttackCircle") || other.CompareTag(gameObject.tag))
            return;
        LifeComponent lifeComponent = other.GetComponent<LifeComponent>();
        if (gameObject.CompareTag("Zombie") && lifeComponent !=null)
        {
            lifeComponent.onLifeEnds?.Invoke("A zombie");
            return;
        }
        
        if (lifeComponent != null)
        {
            lifeComponent.onLifeEnds?.Invoke(initiator.GetComponent<ActorInformationController>().GetName());
            initiator.EventIfKillSomeone();
            SelfDestroy();
        }
    }

    public void InitIAttacker(ActorAttacker master)
    {
        initiator = master;
    }

    protected virtual void SelfDestroy()
    {
        Destroy(gameObject);
    }
}