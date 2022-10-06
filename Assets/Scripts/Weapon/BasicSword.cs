using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicSword : Weapon , IAttackable
{
    private Collider[] colliders = new Collider[3];
    [SerializeField]
    LayerMask layerMask;
    [SerializeField]
    float attackRange;
    public override void WeaponAttack()
    {
        for(int i = 0; i < colliders.Length; i++) colliders[i] = null;
        Physics.OverlapSphereNonAlloc(transform.position, attackRange, colliders,layerMask,QueryTriggerInteraction.Collide);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i]!=null)
            {
                IDamageable target = colliders[i].GetComponent<IDamageable>();
                target?.Hit(this, transform.position);           
            }
        }
    }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


    public void Attack(IDamageable target)
    {
        target.DirectHit(damage);
    }


}
