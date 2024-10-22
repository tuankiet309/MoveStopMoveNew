using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enum;

public class Projectile : MonoBehaviour, IProjectile
{
    float distanceTilDie = 0f;
    float distanceComeBackAccept = 1f;
    Enum.WeaponType weaponType;
    private Vector3 flyDirection;
    ActorAttacker actorAttacker;
    private Vector3 tempoScale;

    [SerializeField] DamageComponent damageComponent;
    [SerializeField] Rigidbody rb;

    public WeaponType WeaponType { get => weaponType; private set { } }
    public float DistanceTilDie { get => distanceTilDie; private set { } }

    private void Awake()
    {
        distanceTilDie = CONSTANT_VALUE.FIRST_CIRCLE_RADIUS + CONSTANT_VALUE.OFFSET_DISTANCE;
        tempoScale = transform.localScale * 3f;
    }

    public virtual void InitForProjectileToThrow(ActorAttacker Initiator,Enum.WeaponType weapon, float buff)
    { 
        actorAttacker = Initiator;
        damageComponent.InitIAttacker(Initiator);
        distanceTilDie += buff;
        this.weaponType = weapon;
    }
   
    public virtual void FlyToPos(Vector3 Enemy, bool isSpeacial)
    {
        if(actorAttacker.gameObject.CompareTag("Player"))
        {
            Debug.Log(distanceTilDie);
        }
        Vector3 flyDirection = Enemy - transform.position;
        flyDirection = new Vector3(flyDirection.x,transform.position.y,flyDirection.z).normalized;
        if (isSpeacial)
            StartCoroutine(SpeacialFly(transform.position, flyDirection.normalized));
        else
            StartCoroutine(Fly(transform.position,flyDirection.normalized));
    }
    IEnumerator Fly(Vector3 initDistance,Vector3 flyDir)
    {
        while(distanceTilDie  > Vector3.Distance(transform.position, initDistance))
        {
            rb.velocity = flyDir.normalized * (CONSTANT_VALUE.PROJECTILE_FLY_SPEED );
            if (weaponType == WeaponType.Rotate || weaponType == WeaponType.Comeback)
            { 
                float rotationSpeed = CONSTANT_VALUE.PROJECTILE_ROTATE_SPEED;
                Quaternion rotation = Quaternion.Euler(0f, rotationSpeed, 0f); 
                rb.MoveRotation(rb.rotation * rotation); 
            }
            yield return null;
        }
        if(weaponType == WeaponType.Comeback)
        {
            while (distanceComeBackAccept < Vector3.Distance(transform.position, actorAttacker.transform.position ))
            {
                Vector3 returnDirection = (actorAttacker.transform.position - transform.position).normalized;
                rb.velocity = returnDirection.normalized * CONSTANT_VALUE.COMEBACK_FLY_DISTANCE;
                if (weaponType == WeaponType.Rotate || weaponType==  WeaponType.Comeback)
                {
                    float rotationSpeed = CONSTANT_VALUE.PROJECTILE_ROTATE_SPEED;
                    Quaternion rotation = Quaternion.Euler(0f, rotationSpeed, 0f);
                    rb.MoveRotation(rb.rotation * rotation);
                }

                yield return null;
            }
        }
        SelfDestroy();
    }
    IEnumerator SpeacialFly(Vector3 initDistance, Vector3 flyDir)
    {
        while (distanceTilDie*2.5f > Vector3.Distance(transform.position, initDistance))
        {
            rb.velocity = flyDir.normalized * (CONSTANT_VALUE.PROJECTILE_FLY_SPEED);
            transform.localScale = Vector3.Lerp(transform.localScale, tempoScale, 0.03f);
            if (weaponType == WeaponType.Rotate || weaponType == WeaponType.Comeback)
            {
                transform.localRotation = Quaternion.LookRotation(-flyDir);
            }
            yield return null;
        }
       
        SelfDestroy();
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

}
