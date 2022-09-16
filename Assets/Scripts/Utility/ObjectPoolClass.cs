using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolClass : Singleton<ObjectPoolClass>
{
   [System.Serializable]
    public class PoolInfo
    {
        public InPoolObj obj;
        public int start;
        public int add;
    }
    [SerializeField]
    PoolInfo[] poolInfos;
    [SerializeField]
    Dictionary<string, PoolInfo> poolInfoDic = new Dictionary<string, PoolInfo>();
    Transform active;
    Transform deactive;
    Dictionary<string, Queue<InPoolObj>> poolDict = new Dictionary<string, Queue<InPoolObj>>();

    protected override void Awake()
    {
        base.Awake();
        for(int i = 0; i < poolInfos.Length; i++)
        {
            poolInfoDic.Add(poolInfos[i].obj.name,poolInfos[i]);
            poolDict.Add(poolInfos[i].obj.name+"(Clone)", new Queue<InPoolObj>());
            AddStart(poolInfos[i].obj.name);
        }
        active = new GameObject("activePool").transform;
        deactive = new GameObject("deactivePool").transform;
        active.gameObject.SetActive(true);
        deactive.gameObject.SetActive(false);
    }


    public void Add(string name)
    {
        PoolInfo temp;
        poolInfoDic.TryGetValue(name, out temp);
        if(temp == null)
        {
            Debug.Log("풀 목록에 존재하지 않는 오브젝트");
            return;
        }
        for(int i = 0; i < temp.add; i++)
        {
            InPoolObj tempobj = Instantiate(temp.obj, deactive);
            tempobj.gameObject.SetActive(false);
            poolDict[name + "(Clone)"].Enqueue(tempobj);
        }
    }

    public void AddStart(string name)
    {
        PoolInfo temp;
        poolInfoDic.TryGetValue(name, out temp);
        for (int i = 0; i < temp.start; i++)
        {
            InPoolObj tempobj = Instantiate(temp.obj, deactive);
            tempobj.gameObject.SetActive(false);
            poolDict[name + "(Clone)"].Enqueue(tempobj);
        }
    }

    public Transform Call(string name, Vector3 positon,Vector3 rotate)
    {
        if(!poolDict.ContainsKey(name + "(Clone)"))
        {
            return null;
        }
        if(poolDict[name + "(Clone)"].Count == 0)
        {
            Add(name);
        }
        Transform temp = poolDict[name + "(Clone)"]?.Dequeue().transform;
        temp.position = positon;
        temp.rotation = Quaternion.Euler(rotate);
        temp.SetParent(active);
        return temp;
    }

    public void Return(InPoolObj obj)
    {
        if (!poolDict.ContainsKey(obj.gameObject.name))
        {
            return;
        }
        obj.transform.SetParent(deactive);
        poolDict[obj.gameObject.name].Enqueue(obj);

    }





}
