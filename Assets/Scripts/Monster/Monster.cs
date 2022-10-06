using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;
using System;
using MonsterState;
using JetBrains.Annotations;

public class Monster : Character,IReturnable,IAttackable
{
    public enum MonState { Idle,Chase,Ready,Attack,Hit,Dead}
    protected StateMachine<MonState, Monster> stateMachine;
    [SerializeField]
    protected float scanRange;
    protected float attackRange;
    [HideInInspector]
    public float attackDelay;
    [SerializeField]
    protected float attackCooltime;
    public float damage;
    [Header("Parts")]
    public Transform direction;
    [SerializeField]
    protected Transform graphic;
    protected NavMeshAgent navMeshAgent;
    [HideInInspector]
    public Animator animator;
    [SerializeField]
    protected LayerMask mask;
    public bool isNamed;
    [HideInInspector]
    public float attackDelayCount = 0;
    protected float attackCooltimeCount = 0;
    [HideInInspector]
    public bool isAttackCooltime;
    protected Collider[] attackBox = new Collider[1];
    public UnityAction[] attackActions = new UnityAction[6];
    [SerializeField]
    protected List<MonsterAttackPattern> patterns = new List<MonsterAttackPattern>();
    [HideInInspector]
    public Character target;
    public Vector3 targetVec;
    public readonly int animation_Ready = Animator.StringToHash("Ready");
    protected bool isSet;

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

    protected virtual void OnEnable() => StartCoroutine(CoChangeState());


    public void StateSet()
    {
        transform.TryGetComponent(out navMeshAgent);
        graphic.TryGetComponent(out animator);
        stateMachine = new StateMachine<MonState, Monster>(this);
        stateMachine.AddState(MonState.Idle, new MonsterState.Idle());
        stateMachine.AddState(MonState.Ready, new MonsterState.Ready());
        stateMachine.AddState(MonState.Chase, new MonsterState.Chase());
        stateMachine.AddState(MonState.Attack, new MonsterState.Attack());
        stateMachine.AddState(MonState.Hit, new MonsterState.Hit());
        stateMachine.AddState(MonState.Dead, new MonsterState.Dead());
        _curHp = _maxHp;
        navMeshAgent.speed = moveSpeed;
        if(patterns.Count==1)
        {
            attackDelay = patterns[0].delay;
            attackCooltime = patterns[0].cooltime;
            attackRange = patterns[0].attackRange;
            for(int i =0; i<attackActions.Length;i++)
            {
                attackActions[i] = patterns[0].attakcAction[i];
            }
        }
        isSet = true;
    }
    public IEnumerator CoChangeState()
    {
        yield return WaitList.isMonsterManagerSet;
        if (!isSet) StateSet();
        ChangeState(MonState.Idle);
    }

    public override void DirectHit(float damage)=> curHp -= damage;
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
            attackCooltimeCount = 0;
            MonsterBehaviourManager.instance.monsterBehaviour -= AttackCooltimeCount;
        }
    }
    public bool MonsterHitCheck() => HitInput != null;
    public void AttackDelayCountReset() => attackDelayCount = 0;
    public virtual void MonsterLookAt()
    {
        direction.LookAt(MonsterBehaviourManager.instance.playerPosition);
        graphic.transform.localRotation = direction.localRotation;
    }
    public virtual void MonsterMove()
    {
        if (navMeshAgent.isOnNavMesh)
        {
            MonsterLookAt();
            navMeshAgent.SetDestination(MonsterBehaviourManager.instance.playerPosition);
            animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);
        }
    }
    public void MonsterNavReset() => navMeshAgent.ResetPath();
    public void MonsterMoveSwitch() => navMeshAgent.enabled = !navMeshAgent.enabled;
    public void ChangeState(MonState state) => stateMachine.ChangeState(state);
    public virtual void Attack(IDamageable target)=> target.DirectHit(damage);


}
