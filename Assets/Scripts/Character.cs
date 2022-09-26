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
    public float moveSpeed;
    public Animator animator;
    public Rigidbody rigi;
    [HideInInspector]
    public List<AsyncState.Type> curStatusEffect = new List<AsyncState.Type>();
    public UnityAction<IDamageable> HitInput;
    protected Vector3 crashVec;
    protected Vector3 moveVec;
    [SerializeField]
    protected float _curHp;
    public virtual float curHp { get { return _curHp; } set { _curHp = value; } }
    [SerializeField]
    protected float _maxHp;
    public virtual float maxHp { get { return _maxHp; } set { _maxHp = value; } }
    public virtual Character Hit(IAttackable attacker, Vector3 attackPosition)
    {
        if (HitInput != null) return null;
        HitInput = attacker.Attack;
        crashVec = -1 * (attackPosition - transform.position).normalized;
        return this;
    }
    public virtual void DirectHit()
    {
        rigi.velocity = Vector3.zero;
        rigi.AddForce(crashVec * 30);
    }

    public virtual bool DeadCheck() => _curHp <= 0;




}
