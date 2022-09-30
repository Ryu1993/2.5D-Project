using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSlime : Monster
{

    protected override void OnEnable()
    {
        base.OnEnable();
        deadEvent += TutorialManager.instance.TutorialMonsterEvent;
        deadEvent += () => this.gameObject.SetActive(false);
    }


    public override void MonsterAttack()
    {
        attackBox[0] = null;
        Physics.OverlapBoxNonAlloc(transform.forward, new Vector3(0.5f, 0.5f, 0.5f), attackBox, Quaternion.identity, mask);
        if (attackBox[0] != null)
        {
            Player target = attackBox[0].GetComponent<Player>();
            target.Hit(this, transform.position);
        }
    }
    public override void Attack(IDamageable target)
    {
        target.DirectHit(damage);
    }



}
