using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class State<T> where T :MonoBehaviour
{
    public bool isCompleted;
    public abstract void Enter(T order);
    public abstract IEnumerator Middle(T order);
    public abstract void Exit(T order);


}
