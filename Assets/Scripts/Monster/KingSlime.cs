using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KingSlime : BossMonster
{
    [System.Serializable]
    private class AttackPattern
    {
        public Pattern pattern;
        public int cooltime;
        public float delay;
        [HideInInspector]
        public int animationHash;
    }

    public enum Pattern { Range,Charge,Jump,Normal}
    private NewObjectPool.PoolInfo bullet;
    private UnityAction selectedAttack;
    [SerializeField]
    private Rigidbody rigi;
    [Header("PatternCooltime(desiSeconds)")]
    [Header("PatternDelay")]
    [SerializeField]
    AttackPattern[] attackPatterns;
    [SerializeField]
    SphereCollider sphereCollider;
    [SerializeField]
    private int rangeAttackCycle;
    private Coroutine rangeAttackCoroutine;
    private AttackPattern curPattern;
    private List<Pattern> enablePatterns = new List<Pattern>();
    private Dictionary<Pattern, AttackPattern> patterns = new Dictionary<Pattern, AttackPattern>();
    private int curPatternCooltime;
    protected override void Awake()
    {
        base.Awake();
        bullet = MonsterBehaviourManager.instance.RequestBullet();
        attackReady = RandomPattern;
        foreach(AttackPattern attackpattern in attackPatterns)
        {
            attackpattern.animationHash = Animator.StringToHash(attackpattern.pattern.ToString());
            patterns.Add(attackpattern.pattern, attackpattern);       
        }
        enablePatterns.Add(Pattern.Range);
        enablePatterns.Add(Pattern.Charge);
        enablePatterns.Add(Pattern.Jump);
        enablePatterns.Add(Pattern.Normal);
    }

    public override void MonsterAttack() => selectedAttack?.Invoke();

    private void RandomPattern()
    {
        curPattern = patterns[enablePatterns[UnityEngine.Random.Range(0, enablePatterns.Count)]];
        animator.SetTrigger(curPattern.animationHash);
        animator.Update(0);
        attackDelay = curPattern.delay;
        curPatternCooltime = curPattern.cooltime;
        AttackSet(curPattern);
    }
    
    private void AttackSet(AttackPattern attackPattern)
    {
        Pattern pattern = attackPattern.pattern;
        attackStart = null;
        selectedAttack = null;
        attackEnd = null;
        if (pattern==Pattern.Normal)
        {
            attackStart = () => sphereCollider.enabled = true;
            attackEnd = () => sphereCollider.enabled = false;
        }
        if(pattern==Pattern.Range)
        {
            selectedAttack = RangeAttack;
            attackEnd = RangeEnd;
        }
        if(pattern == Pattern.Charge)
        {
            attackStart = ChargeStart;
            attackEnd = ChargeEnd;
        }
        if(pattern == Pattern.Jump)
        {
            attackStart = JumpStart;
            selectedAttack = JumpAttack;
            attackEnd = JumpEnd;
        }
    }

    #region RangeAttack
    private void RangeAttack()
    {
        if (rangeAttackCoroutine == null) rangeAttackCoroutine = StartCoroutine(CoRangeAttack());
    }
    private IEnumerator CoRangeAttack()
    {
        SphereRangeAttack();
        for (int i = 0; i < rangeAttackCycle; i++) yield return WaitList.deciSecond;
        rangeAttackCoroutine = null;
    }

    private void SphereRangeAttack()
    {
        MonsterBullet[] bullets = new MonsterBullet[12];
        for (int i = 0; i < bullets.Length; i++)
        {
            NewObjectPool.instance.Call(bullet, transform.position+new Vector3(0,0.5f,0)).TryGetComponent<MonsterBullet>(out bullets[i]);
            bullets[i].OwnerSet(this, 2, 2f, Quaternion.Euler(new Vector3(0, 15 * i, 0)));
            bullets[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    private void RangeEnd()=> StartCoroutine(CoCooltimeCount(Pattern.Range, curPatternCooltime));

    #endregion
    #region ChargeAttack
    private void ChargeStart()
    {
        sphereCollider.enabled = true;
        rigi.isKinematic = false;
        rigi.AddForce(transform.forward * 200);
    }
    private void ChargeEnd()
    {
        sphereCollider.enabled = false;
        rigi.velocity = Vector3.zero;
        rigi.isKinematic = true;
        StartCoroutine(CoCooltimeCount(Pattern.Charge, curPatternCooltime));
    }
    #endregion
    #region JumpAttack
    private void JumpStart()
    {
        rigi.isKinematic = false;
        rigi.AddForce(transform.forward * 300);
    }

    private void JumpAttack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Jump")) return;
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.5f) return;
        sphereCollider.enabled = true;
    }
    private void JumpEnd()
    {
        sphereCollider.enabled = false;
        rigi.velocity = Vector3.zero;
        rigi.isKinematic = true;
        StartCoroutine(CoCooltimeCount(Pattern.Jump, curPatternCooltime));
    }
    #endregion
    private IEnumerator CoCooltimeCount(Pattern pattern,int duration)
    {
        enablePatterns.Remove(pattern);
        for (int i = 0; i < duration; i++) yield return WaitList.deciSecond;
        enablePatterns.Add(pattern);
    }





}
