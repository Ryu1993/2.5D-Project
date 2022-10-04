using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSlime : Monster
{
    [SerializeField]
    private SphereCollider sphereCollider;
   

    protected override void Awake()
    {
        base.Awake();
        attackStart += () => 
        {
            sphereCollider.enabled = true;
            MonsterMoveSwitch();
            navMeshAgent.SetDestination(direction.forward*2);
            navMeshAgent.speed = 8;
                     
        };
        attackEnd += () =>
        {
            sphereCollider.enabled = false;
            navMeshAgent.ResetPath();
            navMeshAgent.speed = moveSpeed;
            MonsterMoveSwitch();
        };
    }

    public override void Attack(IDamageable target)
    {
        target.DirectHit(damage);
        StatusManager.instance.StatusEffectCreate[AsyncState.Type.Sturn](this.target, 2);
    }



}
