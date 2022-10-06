using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSlime : Monster
{




    public override void Attack(IDamageable target)
    {
        target.DirectHit(damage);
    }



}
