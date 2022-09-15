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
        if(poolInfo!=null)
        {
            return poolInfo;
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




    //public void Add(string name)
    //{
    //    PoolInfo temp;
    //    poolInfoDic.TryGetValue(name, out temp);
    //    if (temp == null)
    //    {
    //        Debug.Log("풀 목록에 존재하지 않는 오브젝트");
    //        return;
    //    }
    //    for (int i = 0; i < temp.add; i++)
    //    {
    //        InPoolObj tempobj = Instantiate(temp.obj, deactive);
    //        tempobj.gameObject.SetActive(false);
    //        poolDict[name + "(Clone)"].Enqueue(tempobj);
    //    }
    //}

    //public void AddStart(string name)
    //{
    //    PoolInfo temp;
    //    poolInfoDic.TryGetValue(name, out temp);
    //    for (int i = 0; i < temp.start; i++)
    //    {
    //        InPoolObj tempobj = Instantiate(temp.obj, deactive);
    //        tempobj.gameObject.SetActive(false);
    //        poolDict[name + "(Clone)"].Enqueue(tempobj);
    //    }
    //}

    //public Transform Call(string name, Vector3 positon, Vector3 rotate)
    //{
    //    if (!poolDict.ContainsKey(name + "(Clone)"))
    //    {
    //        return null;
    //    }
    //    if (poolDict[name + "(Clone)"].Count == 0)
    //    {
    //        Add(name);
    //    }
    //    Transform temp = poolDict[name + "(Clone)"]?.Dequeue().transform;
    //    temp.position = positon;
    //    temp.rotation = Quaternion.Euler(rotate);
    //    temp.SetParent(active);
    //    return temp;
    //}

    //public void Return(InPoolObj obj)
    //{
    //    if (!poolDict.ContainsKey(obj.gameObject.name))
    //    {
    //        return;
    //    }
    //    obj.transform.SetParent(deactive);
    //    poolDict[obj.gameObject.name].Enqueue(obj);

    //}




}
