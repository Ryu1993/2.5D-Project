using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSlime : Monster
{
    [SerializeField]
    private SphereCollider sphereCollider;
    Vector3 targetVec;
   

    protected override void Awake()
    {
        base.Awake();
        attackReadyProgress += () =>
        {
            MonsterLookAt();
            targetVec = MonsterBehaviourManager.instance.playerPosition;
        };
        attackStart += () => sphereCollider.enabled = true;
        attackEnd += () => sphereCollider.enabled = false;
    }

    public override void MonsterAttack()
    {
        transform.position = Vector3.Lerp(transform.position, targetVec, Time.fixedDeltaTime * 2);
    }

    public override void Attack(IDamageable target)
    {
        target.DirectHit(damage);
        StatusManager.instance.StatusEffectCreate[AsyncState.Type.Sturn](this.target, 2);
    }



}
