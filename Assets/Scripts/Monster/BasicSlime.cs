using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSlime : Monster
{
    [SerializeField]
    SphereCollider sphereCollider;

    protected override void Awake()
    {
        base.Awake();
        attackStart += () => sphereCollider.enabled = true;
        attackEnd += () => sphereCollider.enabled = false;
    }

    public override void Attack(IDamageable target)
    {
        target.DirectHit(damage);
    }



}
