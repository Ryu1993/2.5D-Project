using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BasicSword : Weapon , IAttackable
{
    private Collider[] colliders = new Collider[3];
    private Vector3 center;
    [SerializeField]
    LayerMask layerMask;


    private void OnEnable()
    {
        center = transform.up - transform.forward / 2;
        type = WeaponType.Sword;
        superArmor = false;
    }
    public override void WeaponAttack()
    {
        for(int i = 0; i < colliders.Length; i++) colliders[i] = null;
        Physics.OverlapBoxNonAlloc(transform.position, new Vector3(0.5f, 0.5f,0.5f), colliders,Quaternion.identity,layerMask);
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
        target.DirectHit();
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(transform.position, new Vector3(1, 1, 1));
    //}

}
