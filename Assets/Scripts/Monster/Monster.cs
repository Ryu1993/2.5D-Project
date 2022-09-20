using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Monster : Character,IReturnable
{
    protected StateMachine<State, Monster> stateMachine;
    public virtual void Awake()
    {
        stateMachine = new StateMachine<State, Monster>(this);
        stateMachine.AddState(State.Trace, new MonsterState.TrasceState());
        stateMachine.AddState(State.Ready, new MonsterState.ReadyState());
        stateMachine.AddState(State.Attack, new MonsterState.AttackState());
        stateMachine.AddState(State.Die,new MonsterState.DieState());
        stateMachine.AddState(State.Hit, new MonsterState.HitState());
        stateMachine.AddState(State.Idle, new MonsterState.IdleState());
        ChangeState(State.Idle);
    }
    public UnityAction dieEvent;
    protected NewObjectPool.PoolInfo poolInfo;
    public void PoolInfoSet(NewObjectPool.PoolInfo pool)
    {
        poolInfo = pool;
        dieEvent += Return;
    }
    public void Return()
    {
        NewObjectPool.instance.Return(this.gameObject, poolInfo);
    }
    public void ChangeState(State state) => stateMachine.ChangeState(state);
}
