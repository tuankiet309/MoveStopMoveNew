using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enum;

public class Projectile : MonoBehaviour, IProjectile
{
    float distanceTilDie = 2f;
    float distanceBuff = 0f;
    float distanceComeBackAccept = 1f;
    Enum.WeaponType weaponType;
    private Vector3 flyDirection;
    ActorAttacker actorAttacker;

    [SerializeField] DamageComponent damageComponent;
    [SerializeField] Rigidbody rb;
    public WeaponType WeaponType { get => weaponType; set => weaponType = value; }
    private void Start()
    {
        distanceTilDie = CONSTANT_VALUE.FIRST_CIRCLE_RADIUS + CONSTANT_VALUE.OFFSET_DISTANCE;
    }
    public virtual void Init(ActorAttacker Initiator,WeaponType weaponType)
    { 
        actorAttacker = Initiator;
        damageComponent.InitIAttacker(Initiator);
        this.weaponType = weaponType;
    }
    public virtual void SetUpWeapon(Enum.WeaponType weaponType, float distanceTilDie, Enum.AttributeBuffs attributeBuffs)
    {
        this.weaponType = weaponType;
        if(attributeBuffs == Enum.AttributeBuffs.Range)
            this.distanceTilDie += distanceTilDie;
    }    
    public virtual void FlyToPos(Vector3 Enemy)
    {
        Vector3 flyDirection = Enemy - transform.position;
        flyDirection = new Vector3(flyDirection.x,transform.position.y,flyDirection.z).normalized;
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
    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
