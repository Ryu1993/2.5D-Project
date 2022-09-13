using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{

    public static T instance;
    //public static T Instance
    //{
    //    get
    //    {
    //        List<T> list = Utility.FindAllObjects<T>();
    //        if(list.Count>1)
    //        {
    //            for(int i = 1; i < list.Count; i++)
    //            {
    //                DestroyImmediate(list[i].gameObject);
    //            }
    //            instance = list[0];
    //        }
    //        if(list.Count == 0)
    //        {
    //            instance = new GameObject(typeof(T).ToString()).AddComponent<T>();
    //        }                 
    //        return instance; 
    //    }
    //}
    protected virtual void Awake()
    {
        if(instance == null)
        {
            instance = this as T;
        }
        else
        {
            DestroyImmediate(this.gameObject);
        }
    }

}
