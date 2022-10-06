using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttack : MonsterAttackPattern
{
    [SerializeField]
    private Transform targetMarker;
    [SerializeField]
    private SphereCollider sphereCollider;

    public override void AnimationHashSet() => animationHash = Animator.StringToHash("Jump");
    public override void AttackReadyStart()
    {
        base.AttackReadyStart();
        TracePlayer();
        targetMarker.gameObject.SetActive(true);
    }
    public override void AttackReadyProgress()=> TracePlayer();

    public override void AttackReadyEnd()
    {
        targetMarker.gameObject.SetActive(false);
    }

    public override void AttackProgress()
    {
        if (monster.animator.GetCurrentAnimatorStateInfo(0).tagHash!=monster.animator_Attack) return;
        if (monster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.4f) return;
        sphereCollider.transform.localScale = Vector3.zero;
        monster.transform.position = monster.targetVec;
        if (monster.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f) return;
        sphereCollider.transform.localScale = Vector3.one;
        sphereCollider.enabled = true;
    }
    public override void AttackEnd()
    {
        sphereCollider.enabled = false;
        base.AttackEnd();
    }
    private void TracePlayer()
    {
        monster.targetVec = MonsterBehaviourManager.instance.playerPosition;
        targetMarker.position = monster.targetVec;
    }



}
