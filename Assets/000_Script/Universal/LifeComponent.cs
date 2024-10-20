 using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class LifeComponent : MonoBehaviour
{
    public UnityEvent<string> onLifeEnds;
    private bool isDead = false;
    private string killerName = "";

    public bool IsDead { get => isDead; private set { isDead = value; } }
    public string KillerName { get => killerName; private set { killerName = value; } }

    private void Start()
    {
        ResetLifeState();
    }
    private void OnEnable()
    {  
        onLifeEnds.AddListener(UpdateDyingState);        
        ResetLifeState();
        ToggleImportantComponents(true);
    }

    private void OnDisable()
    {
        
        onLifeEnds.RemoveAllListeners();
        ResetLifeState();
    }

    private void UpdateDyingState(string killer)
    {
        IsDead = true;
        KillerName = killer;
        ToggleImportantComponents(false);
    }

    private void ToggleImportantComponents(bool isOn)
    {
        
        ActorMovementController actorMovementController = GetComponent<ActorMovementController>();
        Rigidbody rb = GetComponent<Rigidbody>();
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Collider collider = GetComponent<Collider>();

        if (actorMovementController != null)
        {
            actorMovementController.enabled = isOn; 
        }
        if(rb != null)
        {
            rb.velocity = Vector3.zero;
        }
        if (agent != null)
        {
            agent.speed = isOn? agent.speed : 0; 
        }

        if(collider != null)
        {
            collider.enabled = isOn;
        }
    }

    private void ResetLifeState()
    {
        isDead = false; 
        killerName = ""; 
    }
}