using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZomebieAttacker : ActorAttacker
{
    void Start()
    {
        GetComponent<DamageComponent>().InitIAttacker(this);
    }

    // Update is called once per frame
  
}
