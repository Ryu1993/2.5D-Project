using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostObject : MonoBehaviour,IReturnable
{
    [SerializeField]
    NewObjectPool.PoolInfo poolInfo;

    public void OnEnable()
    {
        StartCoroutine(Delay());
    }

    public void PoolInfoSet(NewObjectPool.PoolInfo pool)
    {
        poolInfo = pool;
    }

    public void Return()
    {
        NewObjectPool.instance.Return(gameObject, poolInfo);
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.1f);
        Return();
    }
}
