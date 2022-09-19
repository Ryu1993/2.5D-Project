using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum WeaponType { Sword}
    public WeaponType type;
    public Animator animator;
    public bool superArmor;
    public Player player;
    protected bool deactiveDelay;
    protected int bufferedInput;
    public abstract void WeaponAttack();
    public abstract void WeaponDeactive();


    protected IEnumerator Delay()
    {
        deactiveDelay = true;
        for (int i = 0; i < bufferedInput; i++)
        {
            yield return null;
        }
        deactiveDelay = false;
    }


}
