using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monster : MonoBehaviour
{
    public UnityAction dieEvent;
    public AddressObjectPool.PoolInfo poolInfo;

    public void PoolInfoSet(AddressObjectPool.PoolInfo pool)
    {
        poolInfo = pool;
        dieEvent += DieReturn;
    }
    protected void DieReturn()
    {
        AddressObjectPool.instance.Return(this.gameObject, poolInfo);
    }

}
