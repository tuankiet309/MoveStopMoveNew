using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class LifeComponent : MonoBehaviour
{
    [SerializeField] protected SkinnedMeshRenderer actorMeshRenderer;
    protected int health = 1;

    public UnityEvent<string> onLifeEnds;
    protected bool isDead = false;
    protected string killerName = "";

    public bool IsDead { get => isDead; protected set { isDead = value; } }
    public string KillerName { get => killerName; protected set { killerName = value; } }

    protected virtual void Start()
    {
        ResetLifeState();
    }

    protected virtual void OnEnable()
    {
        onLifeEnds.AddListener(UpdateDyingState);
        ResetLifeState();
        ToggleImportantComponents(true);
    }

    protected virtual void OnDisable()
    {
        onLifeEnds.RemoveAllListeners();
        ResetLifeState();
    }

    protected virtual void UpdateDyingState(string killer)
    {
        IsDead = true;
        KillerName = killer;
        ToggleImportantComponents(false);
    }

    protected virtual void ToggleImportantComponents(bool isOn)
    {
        ActorMovementController actorMovementController = GetComponent<ActorMovementController>();
        Rigidbody rb = GetComponent<Rigidbody>();
        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        Collider collider = GetComponent<Collider>();

        if (actorMovementController != null)
        {
            actorMovementController.enabled = isOn;
        }
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
        }
        if (agent != null)
        {
            agent.speed = isOn ? agent.speed : 0;
        }

        if (collider != null)
        {
            collider.enabled = isOn;
        }
    }
    public virtual bool DamageHealth(string attackerName)
    {
        health -= 1;
        if(health <=0)
        {
            if(!gameObject.CompareTag("Zombie"))
                PlayDyingSound();
            ParticleSpawner.Instance.PlayParticle(transform.position + Vector3.up,actorMeshRenderer.sharedMaterial);
            onLifeEnds?.Invoke(attackerName);
            return true;
        }
        else
            return false;
    }

    protected virtual void ResetLifeState()
    {
        isDead = false;
        killerName = "";
    }
    protected virtual void PlayDyingSound()
    {
        SoundList soundList = SoundManager.Instance.SoundLists.FirstOrDefault(sound => sound.SoundListName == Enum.SoundType.Dead);
        int random = Random.Range(0, soundList.Sounds.Length);
        AudioClip clip = soundList.Sounds[random];
        SoundManager.Instance.PlayThisOnWorld(clip, 0.25f, transform.position);
    }
}