using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastAttackBehaviour : AttackBehaviourBase
{
    protected bool isActivated;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.parent.TryGetComponent(out player);
        player.comboCount++;
        isActivated = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!isActivated)
        {
            if(stateInfo.normalizedTime>0.5f)
            {
                player.weaponContainer.weaponAttack(player.comboCount);
                isActivated = true;
            }
        }
    }

}
