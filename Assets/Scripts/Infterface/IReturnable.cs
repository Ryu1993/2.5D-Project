using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReturnable
{
    public void PoolInfoSet(NewObjectPool.PoolInfo pool);
    public void Return();


}
