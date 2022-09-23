using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Monster : Character,IReturnable
{
    protected StateMachine<State, Monster> stateMachine;
    [SerializeField]
    NavMeshAgent navMeshAgent;
    [SerializeField]
    LayerMask mask;
    Collider[] colliders = new Collider[1];
    public bool isTargetOn;

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

    public void ScanTarget()
    {
        isTargetOn = false;
        colliders[0] = null;
        Physics.OverlapSphereNonAlloc(transform.position, 2f, colliders, mask);
        if (colliders[0] != null)
        {
            if((transform.position - colliders[0].transform.position).magnitude > 1) isTargetOn = true;
        }
    }

    public void MonsterMove() => navMeshAgent.destination = GameManager.instance.PlayerPosition;

}
