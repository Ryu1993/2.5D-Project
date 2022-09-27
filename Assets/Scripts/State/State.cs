using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class State<T> where T : MonoBehaviour
{
    protected T order;
    public abstract void Enter(T order);
    public abstract void Exit();
    public abstract void Progress();



}
