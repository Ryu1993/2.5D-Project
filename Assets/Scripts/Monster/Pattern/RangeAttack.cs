using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class RangeAttack : MonsterAttackPattern
{
    protected NewObjectPool.PoolInfo bulletKey;
    [SerializeField]
    protected float rangeSpeed;
    [SerializeField]
    protected float rangeDuration;
    [SerializeField]
    protected float rangeDegree;
    [SerializeField]
    protected Vector3 bulletScale;
    [SerializeField]
    protected int bulletNum;
    [SerializeField]
    protected float rangeCycle;
    protected Coroutine rangeAttack;
    protected MonsterBullet[] bullets;
    protected WaitForSeconds wait;


    public override void AnimationHashSet()
    {
        animationHash = Animator.StringToHash("Range");
        bulletKey = MonsterBehaviourManager.instance.RequestBullet();
        bullets = new MonsterBullet[bulletNum];
        wait = new WaitForSeconds(rangeCycle);
    }

    public override void AttackProgress()
    {
        if (rangeAttack == null) rangeAttack = StartCoroutine(CoRangeAttack());
    }
    public override void AttackEnd()
    {

        StopCoroutine(rangeAttack);
        rangeAttack = null;
        base.AttackEnd();
    }
    protected IEnumerator CoRangeAttack()
    {
        while (true)
        {
            CreateBullet();
            yield return wait;
        }      
    }

    protected virtual void CreateBullet()
    {
        for (int i = 0; i < bullets.Length; i++)
        {
            NewObjectPool.instance.Call(bulletKey, transform.position).TryGetComponent<MonsterBullet>(out bullets[i]);
            bullets[i].OwnerSet(monster, rangeSpeed, rangeDuration, Quaternion.Euler(new Vector3(0, rangeDegree * i, 0)));
            bullets[i].transform.localScale = bulletScale;
        }
    }


}
