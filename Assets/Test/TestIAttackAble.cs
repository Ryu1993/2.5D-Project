using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestIAttackAble : Monster, IAttackable
{
    Player player;

    public void Attack(IDamageable target)
    {
        target.DirectHit();
        if (player == null) return;
        StatusManager.instance.StatusEffectCreate[AsyncState.Type.Burn]?.Invoke(player,3);
    }

    public void OnEnable()
    {
        ChangeState(MonState.Idle);
        //Invoke("MonsterTestDead", 4f);
    }

    public void MonsterTestDead() => curHp = 0;

}
