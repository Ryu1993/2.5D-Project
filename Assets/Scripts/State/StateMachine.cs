using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T1, T2> where T2 : MonoBehaviour
{
    private T2 Order;
    private State<T2> curState;
    private Dictionary<T1, State<T2>> states;
    private Coroutine curCoroutine;

    public StateMachine(T2 Owner)
    {
        this.Order = Owner;
        curState = null;
        states = new Dictionary<T1, State<T2>>();
    }

    public void AddState(T1 type, State<T2> state)
    {
        states.Add(type, state);
    }

    public void ChangeState(T1 type)
    {
        Order.StartCoroutine(CoChangeState(type));
    }
    public IEnumerator CoChangeState(T1 type)
    {
        if(curCoroutine!=null) while (!curState.isCompleted) yield return null;
        if (curState != null) curState.Exit(Order);
        curState = states[type];
        curState.Enter(Order);
        curCoroutine = Order.StartCoroutine(curState.Middle(Order));
    }


}
