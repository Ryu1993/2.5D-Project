using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeAttack : MonsterAttackPattern
{
    [SerializeField]
    private SphereCollider sphereCollider;
    [SerializeField]
    private Transform chargeDirection;
    [SerializeField]
    private float chargeSpeed;

    public override void AnimationHashSet() => animationHash = Animator.StringToHash("Charge");
    public override void AttackStart()=> sphereCollider.enabled = true;
    public override void AttackProgress()=> transform.position = Vector3.Lerp(transform.position, monster.targetVec, Time.fixedDeltaTime * chargeSpeed);
    public override void AttackEnd()
    {
        sphereCollider.enabled = false;
        base.AttackEnd();
    }
    public override void AttackReadyStart()
    {
        base.AttackReadyStart();
        chargeDirection.position = sphereCollider.transform.position;
        chargeDirection.gameObject.SetActive(true);
    }
    public override void AttackReadyProgress()
    {
        if (monster.attackDelayCount <= delay * 0.7f)
        {
            monster.MonsterLookAt();
            monster.targetVec = MonsterBehaviourManager.instance.playerPosition;
            chargeDirection.rotation = monster.direction.rotation;
        }
        else chargeDirection.gameObject.SetActive(false);
    }

}
