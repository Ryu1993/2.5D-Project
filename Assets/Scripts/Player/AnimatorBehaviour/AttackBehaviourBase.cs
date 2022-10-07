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
        player.weaponContainer.weaponAttack();
        player.weaponContainer.weaponVFX.gameObject.SetActive(true);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player.weaponContainer.weaponVFX.gameObject.SetActive(false);
    }

}
