using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Monster : Character,IReturnable
{
    public enum MonState { Idle,Chase,Ready,Attack,Hit,Dead}
    protected StateMachine<MonState, Monster> stateMachine;
    [SerializeField]
    protected NavMeshAgent navMeshAgent;
    [SerializeField]
    protected LayerMask mask;
    [SerializeField]
    protected float scanRange;
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    protected float attackDelay;
    [SerializeField]
    protected float attackDelayCount = 0;
    public int isAttack { get; private set; }
    public bool isAnimation;
    private Collider[] attackBox = new Collider[1];


    #region Returnable
    public UnityAction deadEvent;
    protected NewObjectPool.PoolInfo poolInfo;
    public void PoolInfoSet(NewObjectPool.PoolInfo pool)
    {
        poolInfo = pool;
        deadEvent += Return;
    }
    public void Return()
    {
        _curHp = _maxHp;
        HitInput = null;
        stateMachine.ResetState();
        NewObjectPool.instance.Return(this.gameObject, poolInfo);
    }
    #endregion

    private void Awake() => StateSet();
    private void OnEnable() => ChangeState(MonState.Idle);


    public void StateSet()
    {
        isAttack = Animator.StringToHash("isAttack");
        stateMachine = new StateMachine<MonState, Monster>(this);
        stateMachine.AddState(MonState.Idle, new MonsterState.Idle());
        stateMachine.AddState(MonState.Ready, new MonsterState.Ready());
        stateMachine.AddState(MonState.Chase,new MonsterState.Chase());
        stateMachine.AddState(MonState.Attack,new MonsterState.Attack());
        stateMachine.AddState(MonState.Hit,new MonsterState.Hit());
        stateMachine.AddState(MonState.Dead,new MonsterState.Dead());
        _curHp = _maxHp;
    }
    public bool ScanTarget()
    {
        if ((transform.position - MonsterBehaviourManager.instance.playerPosition).magnitude <= scanRange) return true;
        return false;
    }
    public virtual bool AttackableCheck()
    {
        AttackRangeCheck();
        if(attackBox[0]!=null) return true;
        return false;
    }
    protected virtual void AttackRangeCheck()
    {
        attackBox[0] = null;
        Physics.OverlapBoxNonAlloc(transform.position, new Vector3(attackRange, attackRange, attackRange), attackBox, Quaternion.identity, mask);
        if(attackBox[0]!=null)
        {
            float temp = Vector3.Dot(transform.forward,(attackBox[0].transform.position-transform.forward).normalized);
            if (temp < 0) attackBox[0] = null;
        }
    }
    public bool AttackDelayCount()
    {
        attackDelayCount += 0.02f;
        if(attackDelayCount>=attackDelay) return true;
        return false;
    }
    public bool MonsterHitCheck() => HitInput != null;
    public void AttackDelayCountReset() => attackDelayCount = 0;
    public void MonsterMove() { if (navMeshAgent.isOnNavMesh) navMeshAgent.SetDestination(MonsterBehaviourManager.instance.playerPosition); }
    public void MonsterMoveSwitch() => navMeshAgent.enabled = !navMeshAgent.enabled;
    public void MonsterKinematicSwitch() => rigi.isKinematic = !rigi.isKinematic;
    public void MonsterAnimationkStateSwitch() => isAnimation = !isAnimation;
    public void ChangeState(MonState state) => stateMachine.ChangeState(state);



}
