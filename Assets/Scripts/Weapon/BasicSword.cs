using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSword : Weapon
{
 
    public void Awake()
    {
        type = WeaponType.Sword;
        superArmor = false;
        bufferedInput = 3;
    }

    public override void WeaponAttack()
    {
        animator.SetBool("Action", true);
    }
    public override void WeaponDeactive()
    {
        if(!deactiveDelay)
        {
            animator.SetBool("Action", false);
            Coroutine coroutine = StartCoroutine(Delay());
        }    
    }

}
