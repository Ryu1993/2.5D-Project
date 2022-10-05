using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicSword : Weapon , IAttackable
{
    private Collider[] colliders = new Collider[3];
    [SerializeField]
    LayerMask layerMask;
    public override void WeaponAttack()
    {
        Debug.Log("attack");
        for(int i = 0; i < colliders.Length; i++) colliders[i] = null;
        Physics.OverlapBoxNonAlloc(attackPoint.position, new Vector3(0.5f, 0.5f,0.5f), colliders,Quaternion.identity,layerMask,QueryTriggerInteraction.Collide);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i]!=null)
            {
                IDamageable target = colliders[i].GetComponent<IDamageable>();
                target?.Hit(this, transform.position);           
            }
        }
    }

    public void Attack(IDamageable target)
    {
        target.DirectHit(damage);
    }


}
