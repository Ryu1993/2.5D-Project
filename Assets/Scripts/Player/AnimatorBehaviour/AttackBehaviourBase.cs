using Cysharp.Threading.Tasks.CompilerServices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviourBase : StateMachineBehaviour
{
    private Player player;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.parent.TryGetComponent(out player);
        player.comboCount++;
        player.weaponContainer.weaponAttack();
        player.weaponContainer.weaponVFX.transform.localRotation = Quaternion.Euler(0, 0, -45 * player.comboCount+95);
        player.weaponContainer.vfx.Play();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(player.animator_Continue, false);
    }
}
