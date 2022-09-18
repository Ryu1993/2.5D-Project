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
    public float curHp;
    public Animator animator;
    public List<AsyncState.Type> curStatusEffect = new List<AsyncState.Type>();
    public UnityAction<IDamageable> HitInput;
    protected Vector3 crashVec;
    protected Vector3 moveVec;


    public virtual void Hit(IAttackable attacker, Vector3 attackPosition)
    {
        if (HitInput != null) return;
        HitInput = attacker.Attack;
        crashVec = -1 * (attackPosition - transform.position).normalized;
    }
    public virtual void DirectHit()
    {

    }


}
