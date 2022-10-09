using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;

public class BasicSword : Weapon
{
    [SerializeField]
    private VisualEffect vfx;

    public override void WeaponAttack(int combo)
    {
        realDamage = damage * combo;
        transform.parent.localRotation = Quaternion.Euler(0, 0, -45 * combo + 95);
        if(combo == 4)
        {
            transform.parent.localRotation = Quaternion.Euler(0, 0, 85);
        }
        vfx.Play();
        for(int i = 0; i < colliders.Length; i++) colliders[i] = null;
        Physics.OverlapSphereNonAlloc(transform.position, attackRange, colliders,layerMask,QueryTriggerInteraction.Collide);
        foreach(Collider collider in colliders)
        {
            if(collider!=null)
            {
                collider.TryGetComponent(out IDamageable target);
                target?.Hit(this, transform.position);
            }
        }
    }
    

}
