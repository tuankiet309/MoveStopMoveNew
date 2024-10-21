using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    void InitForProjectileToThrow(ActorAttacker Initiator,Enum.WeaponType weaponType, float distanceBuffEach);
    void FlyToPos(Vector3 Enemy, bool isSpeacial);

}
