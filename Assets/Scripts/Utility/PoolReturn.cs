using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolReturn : MonoBehaviour, IReturnable
{
    private NewObjectPool.PoolInfo home;
    public void PoolInfoSet(NewObjectPool.PoolInfo pool)=>home=pool;
    public void Return() => NewObjectPool.instance.Return(this.gameObject, home);

}
