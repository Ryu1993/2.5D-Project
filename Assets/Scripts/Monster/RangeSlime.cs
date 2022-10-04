using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeSlime : Monster
{
    private NewObjectPool.PoolInfo bullet;
    [Header("deciSeconds")]
    [SerializeField]
    private int rangeCooltime;
    [SerializeField]
    private float rangeSpeed;
    [SerializeField]
    private float rangeDuration;
    private Coroutine rangeAttack;
    MonsterBullet[] bullets = new MonsterBullet[6];

    protected override void Awake()
    {
        base.Awake();
        bullet = MonsterBehaviourManager.instance.RequestBullet();
    }

    public override void MonsterAttack()
    {
        if (rangeAttack == null) rangeAttack = StartCoroutine(CoRangeAttack());
    }
    public override void Attack(IDamageable target)
    {
        target.DirectHit(damage);
    }

    private IEnumerator CoRangeAttack()
    {
        RangeAttack();
        for (int i = 0; i < rangeCooltime; i++) yield return WaitList.deciSecond;
        rangeAttack = null;
    }

    private void RangeAttack()
    {
        
        for(int i=0;i<bullets.Length;i++)
        {
            NewObjectPool.instance.Call(bullet, transform.position).TryGetComponent<MonsterBullet>(out bullets[i]);
            bullets[i].OwnerSet(this, rangeSpeed, rangeDuration,Quaternion.Euler(new Vector3(0, 30*i,0)));
        }
    }


}
