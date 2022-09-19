using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicSword : Weapon
{
    public new WeaponType type = WeaponType.Sword;
    public override void WeaponAttack()
    {
        if (!Input.GetMouseButton(0)) { animator.SetBool("Action", false); return; }
        animator.SetBool("Action", true);
        
    }

}
