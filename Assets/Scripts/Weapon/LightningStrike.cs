using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : Weapon
{

    [SerializeField]
    private GameObject particle;
    private NewObjectPool.PoolInfo key;
    private float realRange;

    private void Awake()
    {
        key = NewObjectPool.instance.PoolInfoSet(particle, 3, 1);
        realDamage = damage;
    }

    public override void WeaponAttack(int combo)
    {
        transform.localPosition = new Vector3(0, -8, combo * 2f);
        realRange = attackRange * combo;
        NewObjectPool.instance.Call(key, transform.position).TryGetComponent(out LightningVFX effect);
        effect.ScaleSet(new Vector3(realRange,realRange,realRange));
        for (int i = 0; i < colliders.Length; i++) colliders[i] = null;
        Physics.OverlapSphereNonAlloc(transform.position, realRange, colliders, layerMask, QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            if (collider != null)
            {
                collider.TryGetComponent(out IDamageable target);
                target?.Hit(this, transform.position);
            }
        }
    }

    public override void Attack(IDamageable target)
    {
        target.DirectHit(realDamage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
