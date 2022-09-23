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
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable target = collision.transform.GetComponent<IDamageable>();
        if (target == null) return;
        player = target?.Hit(this, transform.position) as Player;
    }

    



}
