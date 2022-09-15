using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monster : MonoBehaviour,IReturnable
{
    public UnityAction dieEvent;
    public AddressObjectPool.PoolInfo poolInfo;

    public void PoolInfoSet(AddressObjectPool.PoolInfo pool)
    {
        poolInfo = pool;
        dieEvent += Return;
    }
    public void Return()
    {
        AddressObjectPool.instance.Return(this.gameObject, poolInfo);
    }

}
