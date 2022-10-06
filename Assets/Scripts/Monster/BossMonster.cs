using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class BossMonster : Monster
{
    public UnityAction<float> bossHpEvent;
    public override float curHp
    {
        get => base.curHp;
        set
        {
            base.curHp = value;
            bossHpEvent?.Invoke(value);
        }
    }

    [Header("PatternInfo")]
    protected MonsterAttackPattern curPattern;

    public override bool AttackableCheck()
    {
        AttackRangeCheck();
        if (attackBox[0] != null)
        {
            PatternReady();
            return true;
        }
        return false;
    }

    protected override void AttackRangeCheck()
    {
        curPattern = patterns[UnityEngine.Random.Range(0, patterns.Count)];
        attackRange = curPattern.attackRange;
        base.AttackRangeCheck();
    }

    protected void PatternReady()
    {

        attackDelay = curPattern.delay;
        for (int i = 0; i < attackActions.Length; i++)
        {
            attackActions[i] = null;
            attackActions[i] = curPattern.attakcAction[i];
        }
    }
    public IEnumerator CoCooltimeCount(MonsterAttackPattern pattern, int duration)
    {
        patterns.Remove(pattern);
        for (int i = 0; i < duration; i++) yield return WaitList.oneSecond;
        patterns.Add(pattern);
    }


}
