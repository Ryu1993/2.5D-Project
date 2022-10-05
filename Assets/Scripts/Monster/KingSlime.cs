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
        public float attackRange;
        [HideInInspector]
        public int animationHash;
    }

    public enum Pattern { Range,Charge,Jump,Normal}
    private NewObjectPool.PoolInfo bullet;
    private UnityAction selectedAttack;
    [SerializeField]
    private Rigidbody rigi;
    [Header("PatternInfo")]
    [SerializeField]
    AttackPattern[] attackPatterns;
    [Header("RealCollider")]
    [SerializeField]
    SphereCollider sphereCollider;
    [SerializeField]
    private int rangeAttackCycle;
    [SerializeField]
    private ParticleSystem targetMark;
    [SerializeField]
    private GameObject lineMark;
    private Coroutine rangeAttackCoroutine;
    private AttackPattern curPattern;
    private List<Pattern> enablePatterns = new List<Pattern>();
    private Dictionary<Pattern, AttackPattern> patterns = new Dictionary<Pattern, AttackPattern>();
    private int curPatternCooltime;
    private Vector3 targetVec;
    MonsterBullet[] bullets = new MonsterBullet[12];

    protected override void Awake()
    {
        base.Awake();
        bullet = MonsterBehaviourManager.instance.RequestBullet();
        attackReady = PatternReady;
        foreach (AttackPattern attackpattern in attackPatterns)
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

    protected override void AttackRangeCheck()
    {
        curPattern = null;
        curPattern = patterns[enablePatterns[UnityEngine.Random.Range(0, enablePatterns.Count)]];
        attackRange = curPattern.attackRange;
        base.AttackRangeCheck();
    }

    private void PatternReady()
    {
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
        attackReadyProgress = null;
        attackReadyEnd = null;
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
            attackReadyProgress = ChargeReadyProgress;
            attackStart = ChargeStart;
            selectedAttack = ChargeAttack;
            attackEnd = ChargeEnd;
        }
        if(pattern == Pattern.Jump)
        {
            attackReadyProgress = JumpReadyProgress;
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
        for (int i = 0; i < rangeAttackCycle; i++)
        {
            SphereRangeAttack();
            yield return WaitList.deciSecond;
        }
        rangeAttackCoroutine = null;
    }

    private void SphereRangeAttack()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            NewObjectPool.instance.Call(bullet, transform.position+new Vector3(0,0.5f,0)).TryGetComponent<MonsterBullet>(out bullets[i]);
            bullets[i].OwnerSet(this, 2, 2f, Quaternion.Euler(new Vector3(0, 15 * i, 0)));
            bullets[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }

    private void RangeEnd()
    {
        if (rangeAttackCoroutine != null)
        {
            StopCoroutine(rangeAttackCoroutine);
            rangeAttackCoroutine = null;
        }
        StartCoroutine(CoCooltimeCount(Pattern.Range, curPatternCooltime));
    }

    #endregion
    #region ChargeAttack

    private void ChargeReadyProgress()
    {
        if(!lineMark.activeSelf) lineMark.SetActive(true);
        if (attackDelayCount <= attackDelay * 0.7f)
        {
            MonsterLookAt();
            targetVec = MonsterBehaviourManager.instance.playerPosition;
            lineMark.transform.rotation = direction.rotation;
        }
        else lineMark.SetActive(false);
    }
    private void ChargeStart()
    {
        sphereCollider.enabled = true;

    }
    private void ChargeAttack()
    {
        transform.position = Vector3.Lerp(transform.position, targetVec, Time.fixedDeltaTime * 5);
    }

    private void ChargeEnd()
    {
        sphereCollider.enabled = false;
        StartCoroutine(CoCooltimeCount(Pattern.Charge, curPatternCooltime));
    }
    #endregion
    #region JumpAttack

    private void JumpReadyProgress()
    {
        if(!targetMark.isPlaying)
        {
            targetMark.transform.position = MonsterBehaviourManager.instance.playerPosition;
            targetMark.gameObject.SetActive(true);
        }
        if (attackDelayCount<=attackDelay*0.7f)
        {
            targetVec = MonsterBehaviourManager.instance.playerPosition;
            targetMark.transform.position = Vector3.Lerp(new Vector3(targetVec.x,0.1f,targetVec.z), targetVec, Time.fixedDeltaTime);        
        }
        else if(attackDelayCount>=attackDelay*0.8f)
        {
            targetMark.gameObject.SetActive(false);
        }
    }
    private void JumpAttack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("JumpAttack")) return;
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.3f) return;
        transform.position = targetVec;
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.4f) return;
        sphereCollider.enabled = true;
    }
    private void JumpEnd()
    {
        sphereCollider.enabled = false;
        StartCoroutine(CoCooltimeCount(Pattern.Jump, curPatternCooltime));
    }
    #endregion
    private IEnumerator CoCooltimeCount(Pattern pattern,int duration)
    {
        enablePatterns.Remove(pattern);
        for (int i = 0; i < duration; i++) yield return WaitList.oneSecond;
        enablePatterns.Add(pattern);
    }





}
