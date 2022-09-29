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
    Transform deacitve;


    protected override void Awake()
    {
        base.Awake();
        active = new GameObject("ActivePool").transform;
        deacitve = new GameObject("DeActivePool").transform;
        deacitve.gameObject.SetActive(false);
        AwakePoolInfoSet();
    }
    private void AwakePoolInfoSet()
    {
        if (poolInfos.Length == 0) return;
        foreach (PoolInfo poolInfo in poolInfos) if (poolInfo.MemberCheck()) PoolInfoSet(poolInfo.poolObject, poolInfo.start, poolInfo.add);
    }
    public PoolInfo PoolInfoSet(GameObject gameObject, int start, int add)
    {
        PoolInfo poolInfo = PoolInfoSearch(gameObject);
        if (poolInfo!=null) return poolInfo;
        if(gameObject.GetComponent<IReturnable>()==null) return null;
        poolInfo = new PoolInfo(gameObject, start, add);
        poolInfoDic.Add(poolInfo.poolObject.name, poolInfo);
        poolDic.Add(poolInfo, new Queue<GameObject>());
        Add(poolInfo, true);
        return poolInfo;
    }
    public PoolInfo PoolInfoSearch(GameObject gameObject) => PoolInfoSearch(gameObject.name);
    public PoolInfo PoolInfoSearch(string goName)
    {
        if (poolInfoDic.ContainsKey(goName)) return poolInfoDic[gameObject.name];
        return null;
    }

    private void Add(PoolInfo poolInfo,bool isStart)
    {
        Queue<GameObject> queue = poolDic[poolInfo];
        int addNum;
        if (isStart) addNum = poolInfo.start;
        else addNum = poolInfo.add;
        for(int i = 0; i < addNum; i++)
        {
            GameObject go = Instantiate(poolInfo.poolObject, deacitve);
            go.GetComponent<IReturnable>().PoolInfoSet(poolInfo);
            queue.Enqueue(go);
        }
    }
    #region CallMethod
    public Transform Call(PoolInfo poolInfo,Transform parent,Vector3 position) { return Call(poolInfo, parent, position, Quaternion.identity); }
    public Transform Call(PoolInfo poolInfo, Vector3 positon, Quaternion rotate) { return Call(poolInfo, active, positon, rotate); }
    public Transform Call(PoolInfo poolInfo, Transform transform) { return Call(poolInfo, transform, Quaternion.identity); }
    public Transform Call(PoolInfo poolInfo, Vector3 positon) { return Call(poolInfo, active, positon, Quaternion.identity); }
    public Transform Call(PoolInfo poolInfo) { return Call(poolInfo, active, Quaternion.identity); }
    public Transform Call(PoolInfo poolInfo,Transform parent,Vector3 position,Quaternion rotate)
    {
        Queue<GameObject> pool = poolDic[poolInfo];
        if (pool.Count == 0) Add(poolInfo,false);
        Transform callObject = pool.Dequeue().transform;
        callObject.position = position;
        callObject.rotation = rotate;
        callObject.SetParent(parent);
        return callObject;
    }
    public Transform Call(PoolInfo poolInfo,Transform parent,Quaternion rotate)
    {
        Queue<GameObject> pool = poolDic[poolInfo];
        if (pool.Count == 0) Add(poolInfo,false);
        Transform callObject = pool.Dequeue().transform;
        callObject.rotation = rotate;
        callObject.SetParent(parent);
        return callObject;
    }


    #endregion
    public void Return(GameObject gameObject, PoolInfo poolInfo)
    {
        poolDic[poolInfo].Enqueue(gameObject);
        gameObject.transform.SetParent(deacitve, false);
    }

}
