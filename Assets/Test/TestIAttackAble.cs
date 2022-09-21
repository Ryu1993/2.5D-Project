using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIAttackAble : Monster, IAttackable
{
    Character character;
 
    public void Attack(IDamageable target)
    {
        target.DirectHit();
        if (character == null) return;
        new AsyncState.Burn(3, character); 
    }
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable target = collision.transform.GetComponent<IDamageable>();
        if (target == null) return;
        character = target?.Hit(this, transform.position);
    }



}
