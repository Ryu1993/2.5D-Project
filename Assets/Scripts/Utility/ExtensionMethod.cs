using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ExtensionMethod
{
    //확장 메서드 : 데이터타입에 기능(메서드)를 확장할 수 있는 문법
    //public static 리턴타입 함수명(this 확장할데이터타입 변수명){}


    public static bool IsBetween(this float value, float min, float max)
    {
        return value > min && value < max;
    }

    public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max)
    {
        return new Vector3(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y), Mathf.Clamp(value.z, min.z, max.z));
    }

    public static bool IsCurStateName(this Animator animator, int hash, int layer)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(layer);
        return state.shortNameHash == hash || state.fullPathHash == hash;
    }

    public static bool IsCurStateTag(this Animator animator, int hash, int layer)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(layer);
        return state.tagHash == hash;
    }

    public static T GetComponentParents<T>(this Transform transform) where T : Component
    {
        Transform tempparent = transform.parent;
        T component = null;
        while (tempparent != null)
        {
            if (tempparent.TryGetComponent(out component)) break;
            tempparent = tempparent.parent;
        }
        return component;
    }
    public static List<T> GetComponentsParents<T>(this Transform transform) where T : Component
    {
        Transform tempParent = transform.parent;
        List<T> components = new List<T> ();
        while(tempParent !=null)
        {
            if (tempParent.TryGetComponent(out T component)) components.Add(component);
            tempParent = tempParent.parent;
        }
        return components;
    }

    public static List<T> GetComponentsAll<T>(this Transform transform) where T : Component
    {
        Transform first = transform.parent;
        List<T> components = new List<T>();
        while (true)
        {
            if (first.parent == null) break;
            first = first.parent;
        }
        ChildSearchAll(first, components);
        return components;
    }

    private static void ChildSearchAll<T>(Transform transform, List<T> list) where T : Component
    {
        if (transform.childCount == 0) return;
        for(int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).TryGetComponent(out T component))
            {
                list.Add(component);
            }
            ChildSearchAll(transform.GetChild(i), list);
        }
    }






    public static void CameraShake(this Camera cam)
    {
        
    }


}
