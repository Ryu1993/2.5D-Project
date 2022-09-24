using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class NewObjectPool : Singleton<NewObjectPool>
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
        public bool MemberCheck() { return poolObject != null && start != 0 && add != 0; }
    }
    [SerializeField]
    PoolInfo[] poolInfos;
    Dictionary<string, PoolInfo> poolInfoDic = new Dictionary<string, PoolInfo>();
    Dictionary<PoolInfo, Queue<GameObject>> poolDic = new Dictionary<PoolInfo, Queue<GameObject>>();
    Transform active;
    Transform deactive;
    [SerializeField]
    int PoolInfonum = 0;


    protected override void Awake()
    {
        base.Awake();
        ActiveNoneActiveSet();
        AwakePoolInfoSet();
    }
    private void ActiveNoneActiveSet()
    {
        active = new GameObject("activePool").transform;
        deactive = new GameObject("deactivePool").transform;
        active.gameObject.SetActive(true);
        deactive.gameObject.SetActive(false);
    }
    private void AwakePoolInfoSet()
    {
        if(poolInfos.Length>0)
        {
            foreach(PoolInfo poolInfo in poolInfos)
            {
                if(poolInfo.MemberCheck())
                {
                    if (ReturnableCheck(poolInfo.poolObject)) continue;
                    if (PoolInfoSearch(poolInfo.poolObject)==null)
                    {
                        PoolInfonum++;
                        Debug.Log(poolInfo.poolObject.name);
                        poolInfoDic.Add(poolInfo.poolObject.name, poolInfo);
                        poolDic.Add(poolInfo, new Queue<GameObject>());
                        for (int i = 0; i < poolInfo.start; i++)
                        {
                            GameObject go = Instantiate(poolInfo.poolObject, deactive);
                            go.GetComponent<IReturnable>().PoolInfoSet(poolInfo);
                            poolDic[poolInfo].Enqueue(go);

                        }
                    }
                }
            }
        }
    }
    public PoolInfo PoolInfoSet(GameObject gameObject, int start, int add)
    {
        PoolInfo poolInfo = PoolInfoSearch(gameObject);
        if (poolInfo!=null) return poolInfo;
        if(ReturnableCheck(gameObject)) return null;
        poolInfo = new PoolInfo(gameObject, start, add);
        poolInfoDic.Add(poolInfo.poolObject.name, poolInfo);
        poolDic.Add(poolInfo, new Queue<GameObject>());
        for (int i = 0; i < poolInfo.start; i++)
        {
            GameObject go = Instantiate(poolInfo.poolObject, deactive);
            go.GetComponent<IReturnable>().PoolInfoSet(poolInfo);
            poolDic[poolInfo].Enqueue(go);
        }
        PoolInfonum++;
        return poolInfo;
    }
    private bool ReturnableCheck(GameObject gameObject)
    {
        IReturnable temp = gameObject.GetComponent<IReturnable>();
        if(temp==null) Debug.Log("오브젝트 풀에 적합하지 않은 오브젝트");
        return temp == null;
    }
    public PoolInfo PoolInfoSearch(GameObject gameObject)
    {
        if(poolInfoDic.ContainsKey(gameObject.name)) return poolInfoDic[gameObject.name];
        return null;
    }
    public PoolInfo PoolInfoSearch(string goName)
    {
        if (poolInfoDic.ContainsKey(goName)) return poolInfoDic[gameObject.name];
        return null;
    }

    private void Add(PoolInfo poolInfo)
    {
        Queue<GameObject> queue = poolDic[poolInfo];
        for(int i = 0; i < poolInfo.add; i++)
        {
            GameObject go = Instantiate(poolInfo.poolObject, deactive);
            go.GetComponent<IReturnable>().PoolInfoSet(poolInfo);
            queue.Enqueue(go);
        }
    }
    #region CallMethod
    public Transform Call(PoolInfo poolInfo,Transform parent,Vector3 position)
    {
        Queue<GameObject> pool = poolDic[poolInfo];
        if (pool.Count == 0) Add(poolInfo);
        Transform callObject = pool.Dequeue().transform;
        callObject.position = position;
        callObject.SetParent(parent);
        return callObject;
    }
    public Transform Call(PoolInfo poolInfo,Transform parent,Vector3 position,Quaternion rotate)
    {
        Queue<GameObject> pool = poolDic[poolInfo];
        if (pool.Count == 0) Add(poolInfo);
        Transform callObject = pool.Dequeue().transform;
        callObject.position = position;
        callObject.rotation = rotate;
        callObject.SetParent(parent);
        return callObject;
    }
    public Transform Call(PoolInfo poolInfo,Transform parent,Quaternion rotate)
    {
        Queue<GameObject> pool = poolDic[poolInfo];
        if (pool.Count == 0) Add(poolInfo);
        Transform callObject = pool.Dequeue().transform;
        callObject.position = parent.position;
        callObject.rotation = rotate;
        callObject.SetParent(parent);
        return callObject;
    }
    public Transform Call(PoolInfo poolInfo,Vector3 positon,Quaternion rotate)
    {
        Queue<GameObject> pool = poolDic[poolInfo];
        if(pool.Count == 0) Add(poolInfo);
        Transform callObject = pool.Dequeue().transform;
        callObject.position = positon;
        callObject.rotation = rotate;
        callObject.SetParent(active);
        return callObject;
    }
    public Transform Call(PoolInfo poolInfo,Vector3 positon)
    {
        Queue<GameObject> pool = poolDic[poolInfo];
        if (pool.Count == 0) Add(poolInfo);
        Transform callObject = pool.Dequeue().transform;
        callObject.position = positon;
        callObject.SetParent(active);
        return callObject;
    }
    public Transform Call(PoolInfo poolInfo,Transform transform)
    {
        Queue<GameObject> pool = poolDic[poolInfo];
        if (pool.Count == 0) Add(poolInfo);
        Transform callObject = pool.Dequeue().transform;
        callObject.SetParent(transform,false);
        return callObject;
    }

    public Transform Call(PoolInfo poolInfo)
    {
        Queue<GameObject> pool = poolDic[poolInfo];
        if (pool.Count == 0) Add(poolInfo);
        Transform callObject = pool.Dequeue().transform;
        callObject.SetParent(active);
        return callObject;
    }
    #endregion
    public void Return(GameObject gameObject, PoolInfo poolInfo)
    {
        poolDic[poolInfo].Enqueue(gameObject);
        gameObject.transform.SetParent(deactive,false);
    }

}
