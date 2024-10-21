using UnityEngine.AI;
using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EnemyMovementController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private ActorAttacker attacker;
    [SerializeField] private WeaponComponent weapon;

    private float rotationSpeed;
    private bool isWaiting = false;
    private bool haveWeapon = true;

    private Vector3 rotatedDir;
    private bool isAttacking;
    private Transform targetDes = null;

    public UnityEvent<Vector3> onEnemyMoving;

    private void Awake()
    {
        rotationSpeed = CONSTANT_VALUE.FIRST_ROTATIONSPEED;
        agent.speed = CONSTANT_VALUE.FIRST_MOVESPEED_ENEMY;
    }


    private void OnEnable()
    {
        agent.ResetPath(); 
        agent.isStopped = false; 
        isWaiting = false;
        agent.speed = CONSTANT_VALUE.FIRST_MOVESPEED_ENEMY;
        attacker.onHaveTarget.AddListener(IsTargetInRange);
        attacker.onActorAttack.AddListener(IsAttackingRightNow);
        weapon.onHavingWeapon.AddListener(OnHavingWeapon);
    }
    private void Start()
    {
        SetNewDestination();
    }
    private void OnDisable()
    {
        attacker.onHaveTarget.RemoveListener(IsTargetInRange);
        attacker.onActorAttack.AddListener(IsAttackingRightNow);
        weapon.onHavingWeapon.RemoveListener( OnHavingWeapon);
    }

    private void Update()
    {
        if (isAttacking && targetDes != null && agent.speed !=0)
        {
            RotateTowardsTarget();
        }
        else 
        {
            RotateTowardsMovementDirection();
        }

        if (agent.isStopped == true)
        {
            onEnemyMoving?.Invoke(Vector3.zero);
        }
        else
        {
            onEnemyMoving?.Invoke(agent.velocity);
        }

        if (!isAttacking && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !isWaiting)
        {
            StartCoroutine(WaitAndSetNewDestination());
        }
    }

    private void RotateTowardsMovementDirection()
    {
        if (agent.velocity.sqrMagnitude > 0.1f)
        {
            Vector3 direction = agent.velocity.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void RotateTowardsTarget()
    {
        if (targetDes != null)
        {
            Vector3 direction = (targetDes.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void SetNewDestination()
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Vector3 randomPoint = GetRandomPoint(transform.position, CONSTANT_VALUE.AI_SEARCH_FOR_NEW_DES); 
            agent.SetDestination(randomPoint);
        }
    }
    private Vector3 GetRandomPoint(Vector3 origin, float distance)
    {
        Vector3 randomDirection = Random.insideUnitSphere * distance; 
        randomDirection += origin; 

        NavMeshHit navHit;
       
        if (NavMesh.SamplePosition(randomDirection, out navHit, distance, NavMesh.AllAreas))
        {
            return navHit.position; 
        }
        return origin; 
    }
    private IEnumerator WaitAndSetNewDestination()
    {
        isWaiting = true;
        agent.isStopped = true; 
        float randomWaitTime = Random.Range(1f, 3f); 
        yield return new WaitForSeconds(randomWaitTime);

        agent.isStopped = false; 
        SetNewDestination();
        isWaiting = false;
    }

    private void IsTargetInRange(GameObject target)
    {
        if (target != null && haveWeapon)
        {
            targetDes = target.transform;
            isAttacking = true;
            agent.isStopped = true;
            int randomRang = Random.Range(0, 100);
            if (randomRang < 50)
                agent.SetDestination(GetRandomPoint(transform.position, CONSTANT_VALUE.AI_SEARCH_FOR_NEW_DES));
            else
                agent.SetDestination(transform.position);
        }
        else
        {
            targetDes = null;
            isAttacking = false;
            agent.isStopped = false; 
            
        }
    }
    private void OnHavingWeapon(bool haveWeapon)
    {
        this.haveWeapon = haveWeapon;
    }
    private void IsAttackingRightNow(Vector2 pos)
    {
        rotatedDir = pos;
    }


}