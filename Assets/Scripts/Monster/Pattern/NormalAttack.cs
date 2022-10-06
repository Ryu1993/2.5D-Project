using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttack : MonsterAttackPattern
{
    [SerializeField]
    private SphereCollider sphereCollider;


    public override void AnimationHashSet() => animationHash = Animator.StringToHash("Ready");

    public override void AttackStart() => sphereCollider.enabled = true;
    public override void AttackEnd() => sphereCollider.enabled = false;
 

}
