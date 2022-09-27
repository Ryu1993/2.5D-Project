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



    protected override void Awake()
    {
        base.Awake();
        AwakePoolInfoSet();
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
                        Debug.Log(poolInfo.poolObject.name);
                        poolInfoDic.Add(poolInfo.poolObject.name, poolInfo);
                        poolDic.Add(poolInfo, new Queue<GameObject>());
                        for (int i = 0; i < poolInfo.start; i++)
                        {
                            GameObject go = Instantiate(poolInfo.poolObject, transform);
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
            GameObject go = Instantiate(poolInfo.poolObject, transform);
            go.GetComponent<IReturnable>().PoolInfoSet(poolInfo);
            poolDic[poolInfo].Enqueue(go);
        }
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
            GameObject go = Instantiate(poolInfo.poolObject, transform);
            go.GetComponent<IReturnable>().PoolInfoSet(poolInfo);
            queue.Enqueue(go);
        }
    }
    #region CallMethod
    public Transform Call(PoolInfo poolInfo,Transform parent,Vector3 position) { return Call(poolInfo, parent, position, Quaternion.identity); }
    public Transform Call(PoolInfo poolInfo, Transform parent, Quaternion rotate) { return Call(poolInfo, parent, Vector3.zero, rotate); }
    public Transform Call(PoolInfo poolInfo, Vector3 positon, Quaternion rotate) { return Call(poolInfo, null, positon, rotate); }
    public Transform Call(PoolInfo poolInfo, Transform transform) { return Call(poolInfo, transform, Vector3.zero, Quaternion.identity); }
    public Transform Call(PoolInfo poolInfo, Vector3 positon) { return Call(poolInfo, null, positon, Quaternion.identity); }
    public Transform Call(PoolInfo poolInfo) { return Call(poolInfo, null, Vector3.zero, Quaternion.identity); }
    public Transform Call(PoolInfo poolInfo,Transform parent,Vector3 position,Quaternion rotate)
    {
        Queue<GameObject> pool = poolDic[poolInfo];
        if (pool.Count == 0) Add(poolInfo);
        Transform callObject = pool.Dequeue().transform;
        if(position != Vector3.zero) callObject.position = position;
        callObject.rotation = rotate;
        callObject.gameObject.SetActive(true);
        return callObject;
    }

    #endregion
    public void Return(GameObject gameObject, PoolInfo poolInfo)
    {
        poolDic[poolInfo].Enqueue(gameObject);
        gameObject.SetActive(false);
    }

}
