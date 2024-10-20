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
        if (rb.velocity.y != 0)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ActorAtributeController actorAtributeController = other.GetComponent<ActorAtributeController>();
        if (actorAtributeController != null)
        {
            actorAtributeController.SetHaveUlti();
            onBuffPickup?.Invoke();

        }
    }
}