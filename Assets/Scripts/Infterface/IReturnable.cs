using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReturnable
{
    public void PoolInfoSet(AddressObjectPool.PoolInfo pool);
    public void Return();


}
