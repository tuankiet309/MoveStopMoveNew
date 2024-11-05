using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BuffGift : MonoBehaviour
{
    public UnityEvent onBuffPickup;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private Rigidbody rb;


    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        ActorAttacker attacker = other.GetComponent<ActorAttacker>();
        if (attacker != null)
        {
            attacker.SetUlti();
            onBuffPickup?.Invoke();

        }
    }
}