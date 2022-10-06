using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSlime : Monster
{
    public override void Attack(IDamageable target)
    {
        target.DirectHit(damage);
        StatusManager.instance.StatusEffectCreate[AsyncState.Type.Sturn](this.target, 2);
    }

}
