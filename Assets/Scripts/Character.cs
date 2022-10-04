using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading;
using System.Threading.Tasks;

public class Character : MonoBehaviour, IDamageable
{
    public enum State { Up, Down, Horizontal, HoUp, HoDown, Move, Idle, Hit, Attack, Skill, Dash, Sturn,SuperAttack,Trace,Ready,Die }
    [HideInInspector]
    public State curAction;
    [HideInInspector]
    public List<AsyncState.Type> curStatusEffect = new List<AsyncState.Type>();
    public UnityAction<IDamageable> HitInput;
    protected Vector3 crashVec;
    protected Vector3 moveVec;
    [HideInInspector]
    public bool isHitInput;
    [SerializeField]
    protected float _curHp;
    public virtual float curHp { get { return _curHp; } set { _curHp = value; } }

    public virtual float maxHp { get { return _maxHp; } set { _maxHp = value; } }
    public virtual Character Hit(IAttackable attacker, Vector3 attackPosition)
    {
        if (HitInput!=null) return null;
        HitInput = attacker.Attack;
        crashVec = -1 * (attackPosition - transform.position).normalized;
        return this;
    }
    [Header("Status")]
    public float moveSpeed;
    [SerializeField]
    protected float _maxHp;

    public readonly int animator_Attack = Animator.StringToHash("Attack");
    public readonly int animator_Hit = Animator.StringToHash("Hit");
    public readonly int animator_Dead = Animator.StringToHash("Dead");
    public readonly int animator_Move = Animator.StringToHash("Move");

    public virtual void DirectHit(float damage) => curHp -= damage;

    public virtual bool DeadCheck() => _curHp <= 0;


}
