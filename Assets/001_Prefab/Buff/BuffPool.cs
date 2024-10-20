using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPool : ObjectPoolAbtract<BuffGift>
{
    [SerializeField] BuffGift buffToPool;

    protected override BuffGift CreateObject()
    {
        BuffGift newBuff = Instantiate(buffToPool);
        return newBuff;
    }


}
