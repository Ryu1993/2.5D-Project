using Cysharp.Threading.Tasks.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviourBase : StateMachineBehaviour
{
    protected Player player;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.parent.TryGetComponent(out player);
        player.comboCount++;
        player.weaponContainer.weaponAttack(player.comboCount);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(player.animator_Continue, false);
    }
}
