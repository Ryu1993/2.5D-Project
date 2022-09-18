using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    public void Hit(IAttackable attacker, Vector3 attackPosition);
    public void DirectHit();

}
