using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddressObjectPool : Singleton<AddressObjectPool>
{
    [System.Serializable]
    public class PoolInfo
    {
        public GameObject poolObject;
        public int start;
        public int add;
        public PoolInfo(GameObject _poolObject, int _start, int _add)
        {
            poolObject = _poolObject;
            start = _start;
            add = _add;
        }
    }
    Dictionary<string, PoolInfo> poolInfoDic = new Dictionary<string, PoolInfo>();
    Dictionary<PoolInfo, Queue<GameObject>> poolDic = new Dictionary<PoolInfo, Queue<GameObject>>();
    Transform active;
    Transform deactive;

    protected override void Awake()
    {
        base.Awake();
        ActiveNoneActiveSet();
    }
    private void ActiveNoneActiveSet()
    {
        active = new GameObject("activePool").transform;
        deactive = new GameObject("deactivePool").transform;
        active.gameObject.SetActive(true);
        deactive.gameObject.SetActive(false);
    }

    public PoolInfo PoolInfoSet(GameObject gameObject, int start, int add)
    {
        PoolInfo poolInfo = PoolInfoSearch(gameObject);
        if (poolInfo!=null)
        {
            return poolInfo;
        }
        if(gameObject.GetComponent<IReturnable>()==null)
        {
            Debug.Log("오브젝트 풀에 적합하지 않은 오브젝트");
            return null;
        }
        poolInfo = new PoolInfo(gameObject, start, add);
        poolInfoDic.Add(gameObject.name, poolInfo);
        poolDic.Add(poolInfo, new Queue<GameObject>());
        for (int i = 0; i < poolInfo.start; i++)
        {
            GameObject go = Instantiate(poolInfo.poolObject, deactive);
            poolDic[poolInfo].Enqueue(go);
        }
        return poolInfo;
    }
    public PoolInfo PoolInfoSearch(GameObject gameObject)
    {
        if(poolInfoDic.ContainsKey(gameObject.name))
        {
            return poolInfoDic[gameObject.name];
        }
        return null;
    }

    private void Add(PoolInfo poolInfo)
    {
        Queue<GameObject> queue = poolDic[poolInfo];
        for(int i = 0; i < poolInfo.add; i++)
        {
            GameObject go = Instantiate(poolInfo.poolObject, deactive);
            queue.Enqueue(go);
        }
    }

    #region CallMethod
    public Transform Call(PoolInfo poolInfo,Vector3 positon, Vector3 rotate)
    {
        Queue<GameObject> pool = poolDic[poolInfo];
        if(pool.Count == 0)
        {
            Add(poolInfo);
        }
        Transform callObject = pool.Dequeue().transform;
        callObject.position = positon;
        callObject.rotation = Quaternion.Euler(rotate);
        callObject.SetParent(active);
        return callObject;
    }
    public Transform Call(PoolInfo poolInfo, Vector3 positon)
    {
        Queue<GameObject> pool = poolDic[poolInfo];
        if (pool.Count == 0)
        {
            Add(poolInfo);
        }
        Transform callObject = pool.Dequeue().transform;
        callObject.position = positon;
        callObject.SetParent(active);
        return callObject;
    }

    public Transform Call(PoolInfo poolInfo)
    {
        Queue<GameObject> pool = poolDic[poolInfo];
        if (pool.Count == 0)
        {
            Add(poolInfo);
        }
        Transform callObject = pool.Dequeue().transform;
        return callObject;
    }
    #endregion
    public void Return(GameObject gameObject, PoolInfo poolInfo)
    {
        gameObject.transform.SetParent(deactive);
        poolDic[poolInfo].Enqueue(gameObject);
    }

}
