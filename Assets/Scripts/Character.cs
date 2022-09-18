using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Threading;
using System.Threading.Tasks;

public class Character : MonoBehaviour, IDamageable
{
    public enum State { Up, Down, Horizontal, HoUp, HoDown, Move, Idle, Hit, Attack, Skill, Dash, Sturn }
    [HideInInspector]
    public State curAction;
    public float moveSpeed;
    [HideInInspector]
    public float curHp;
    public Animator animator;
    public Rigidbody rigi;
    [HideInInspector]
    public List<AsyncState.Type> curStatusEffect = new List<AsyncState.Type>();
    public UnityAction<IDamageable> HitInput;
    protected Vector3 crashVec;
    protected Vector3 moveVec;


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


}
