using Cysharp.Threading.Tasks.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviourBase : StateMachineBehaviour
{
    Player player;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.TryGetComponent(out player);
        player.weaponContainer.weaponAttack();
    }
}
