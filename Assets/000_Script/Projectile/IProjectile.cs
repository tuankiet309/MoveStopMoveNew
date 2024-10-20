using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProjectile
{
    void Init(ActorAttacker Initiator,Enum.WeaponType weaponType);
    void FlyToPos(Vector3 Enemy, bool isSpeacial);

}
