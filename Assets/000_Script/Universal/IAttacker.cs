using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttacker
{
    void PrepareToAttack();
    void EventIfKillSomeone();
    void InitWeapon(Projectile weaponToThrow);

}
 