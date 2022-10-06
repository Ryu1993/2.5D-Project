using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Monster))]
public abstract class MonsterAttackPattern : MonoBehaviour
{
    protected Monster monster;
    protected BossMonster boss;
    public int cooltime;
    public float delay;
    public float attackRange;
    [HideInInspector]
    public int animationHash;
    public UnityAction[] attakcAction = new UnityAction[6];

    protected void Awake()
    {
        monster = GetComponent<Monster>();
        monster.TryGetComponent(out boss);
        AnimationHashSet();
        attakcAction[0] = AttackReadyStart;
        attakcAction[1] = AttackReadyProgress;
        attakcAction[2] = AttackReadyEnd;
        attakcAction[3] = AttackStart;
        attakcAction[4] = AttackProgress;
        attakcAction[5] = AttackEnd;
    }
    public abstract void AnimationHashSet();
    public virtual void AttackReadyStart()
    {
        if (!monster.isNamed) return;
        monster.animator.SetTrigger(animationHash);
        monster.animator.Update(0f);
    }
    public virtual void AttackReadyProgress() { }
    public virtual void AttackReadyEnd() { }
    public virtual void AttackStart() { }
    public virtual void AttackProgress() { }
    public virtual void AttackEnd()
    {
        boss?.StartCoroutine(boss.CoCooltimeCount(this, cooltime));
    }



}




