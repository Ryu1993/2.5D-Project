using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monster : MonoBehaviour,IReturnable
{
    public UnityAction dieEvent;
    private NewObjectPool.PoolInfo poolInfo;
    public void PoolInfoSet(NewObjectPool.PoolInfo pool)
    {
        poolInfo = pool;
        dieEvent += Return;
    }
    public void Return()
    {
        NewObjectPool.instance.Return(this.gameObject, poolInfo);
    }

}
