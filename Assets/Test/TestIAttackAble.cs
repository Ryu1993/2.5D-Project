using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestIAttackAble : Monster
{
    Player player;

    public override void Attack(IDamageable target)
    {
        target.DirectHit(0);
        if (player == null) return;
        StatusManager.instance.StatusEffectCreate[AsyncState.Type.Burn]?.Invoke(player,3);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        //Invoke("MonsterTestDead", 4f);
    }

    public void MonsterTestDead() => curHp = 0;

}
