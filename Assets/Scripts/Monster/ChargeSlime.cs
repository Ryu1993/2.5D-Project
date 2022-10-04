using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSlime : Monster
{
    [SerializeField]
    private SphereCollider sphereCollider;
    [SerializeField]
    private Rigidbody rigi;
   

    protected override void Awake()
    {
        base.Awake();
        attackStart += () => 
        {
            sphereCollider.enabled = true;
            rigi.isKinematic = false;
            rigi.AddForce(direction.forward * 200);
            
        };
        attackEnd += () =>
        {
            sphereCollider.enabled = false;
            rigi.velocity = Vector3.zero;
            rigi.isKinematic = true;
            MonsterMoveSwitch();
        };
    }

    public override void Attack(IDamageable target)
    {
        target.DirectHit(damage);
        StatusManager.instance.StatusEffectCreate[AsyncState.Type.Sturn](this.target, 2);
    }



}
