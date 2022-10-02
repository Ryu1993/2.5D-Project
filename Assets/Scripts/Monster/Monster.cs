using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using System;
using MonsterState;

public class Monster : Character,IReturnable,IAttackable
{
    public enum MonState { Idle,Chase,Ready,Attack,Hit,Dead}
    protected StateMachine<MonState, Monster> stateMachine;
    [SerializeField]
    protected float scanRange;
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    protected float attackDelay;
    [SerializeField]
    protected float attackCooltime;
    public float damage;
    [Header("Parts")]
    [SerializeField]
    protected Transform direction;
    [SerializeField]
    protected Transform graphic;
    protected NavMeshAgent navMeshAgent;
    [HideInInspector]
    public Animator animator;
    [SerializeField]
    protected LayerMask mask;
    protected float attackDelayCount = 0;
    protected float attackCooltimeCount = 0;
    protected bool isAttackCooltime;
    protected Collider[] attackBox = new Collider[1];
    public readonly int animator_Ready = Animator.StringToHash("Ready");


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
    protected virtual void OnEnable() => StartCoroutine(CoChangeState());


    public void StateSet()
    {
        navMeshAgent = transform.GetComponent<NavMeshAgent>();
        animator = graphic.GetComponent<Animator>();
        stateMachine = new StateMachine<MonState, Monster>(this);
        stateMachine.AddState(MonState.Idle, new MonsterState.Idle());
        stateMachine.AddState(MonState.Ready, new MonsterState.Ready());
        stateMachine.AddState(MonState.Chase, new MonsterState.Chase());
        stateMachine.AddState(MonState.Attack, new MonsterState.Attack());
        stateMachine.AddState(MonState.Hit, new MonsterState.Hit());
        stateMachine.AddState(MonState.Dead, new MonsterState.Dead());
        _curHp = _maxHp;
        navMeshAgent.speed = moveSpeed;
    }
    public IEnumerator CoChangeState()
    {
        yield return WaitList.isMonsterManagerSet;
        ChangeState(MonState.Idle);
    }

    public override void DirectHit(float damage)=> curHp -= damage;

    public bool ScanTarget()
    {
        if (isAttackCooltime) return false;
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
        Physics.OverlapBoxNonAlloc(direction.position, new Vector3(attackRange, attackRange, attackRange), attackBox, Quaternion.identity, mask);
        if(attackBox[0]!=null)
        {
            float temp = Vector3.Dot(direction.forward,(attackBox[0].transform.position-direction.forward).normalized);
            if (temp>-0.5f&&temp<-0.5f) attackBox[0] = null;
        }
    }
    public bool AttackDelayCount()
    {
        attackDelayCount += Time.fixedDeltaTime;
        if (attackDelayCount>=attackDelay) return true;
        return false;
    }

    public void AttackCooltimeCount()
    {
        isAttackCooltime = true;
        attackCooltimeCount += Time.fixedDeltaTime;
        if (attackCooltimeCount >= attackCooltime)
        {
            isAttackCooltime = false;
            MonsterBehaviourManager.instance.monsterBehaviour -= this.AttackCooltimeCount;
        }
    }


    public virtual void MonsterAttack()
    {



    }

    public bool MonsterHitCheck() => HitInput != null;
    public void AttackDelayCountReset() => attackDelayCount = 0;
    public virtual void MonsterMove()
    {
        if (navMeshAgent.isOnNavMesh)
        { 
            navMeshAgent.SetDestination(MonsterBehaviourManager.instance.playerPosition);
            direction.LookAt(MonsterBehaviourManager.instance.playerPosition);
            graphic.transform.localRotation = direction.localRotation;
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        }
    }
    public void MonsterMoveSwitch() => navMeshAgent.enabled = !navMeshAgent.enabled;
    public void ChangeState(MonState state) => stateMachine.ChangeState(state);

    public virtual void Attack(IDamageable target) { }

}
