using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T1, T2> where T2 : MonoBehaviour
{
    private T2 Order;
    private State<T2> curState;
    private Dictionary<T1, State<T2>> states;

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
        if (curState != null) curState.Exit();
        curState = states[type];
        curState.Enter(Order);
    }
    public void ResetState() => curState = null;

}
