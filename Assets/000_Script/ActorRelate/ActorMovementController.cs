using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ActorMovementController : MonoBehaviour
{
    [SerializeField] protected Stick moveStick;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected ActorAttacker attacker;
    protected float rotateSpeed;
    protected float moveSpeed;

    protected Vector3 moveVelocity = Vector3.zero;
    protected Vector3 rotateDir = Vector3.zero;

    public UnityEvent<Vector3> onActorMoving;

    protected virtual void Awake()
    {
        rotateSpeed = CONSTANT_VALUE.FIRST_ROTATIONSPEED;
        moveSpeed = CONSTANT_VALUE.FIRST_MOVESPEED;
    }

    protected virtual void OnEnable()
    {
        if (moveStick != null)
            moveStick.onThumbstickValueChanged.AddListener(moveStickInputHandler);
        if (attacker != null)
            attacker.onHaveTarget.AddListener(RotateToTarget);
    }

    protected virtual void OnDisable()
    {
        if (moveStick != null)
            moveStick.onThumbstickValueChanged.RemoveListener(moveStickInputHandler);
        if (attacker != null)
            attacker.onHaveTarget.RemoveListener(RotateToTarget);
    }

    protected virtual void Update()
    {
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);
        if (rotateDir != Vector3.zero)
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(rotateDir), rotateSpeed * Time.deltaTime);
    }

    protected virtual void moveStickInputHandler(Vector2 inputValue)
    {
        float x = inputValue.x;
        float z = inputValue.y;
        moveVelocity = new Vector3(x, 0, z).normalized * moveSpeed;
        rotateDir = inputValue == Vector2.zero ? rotateDir : new Vector3(x, 0, z);
        onActorMoving?.Invoke(moveVelocity);
    }

    protected virtual void RotateToTarget(GameObject target)
    {
        if (moveVelocity == Vector3.zero && target != null)
        {
            Vector3 roteToDir = target.transform.position - transform.position;
            rotateDir = new Vector3(roteToDir.x, rotateDir.y, roteToDir.z);
        }
    }
}