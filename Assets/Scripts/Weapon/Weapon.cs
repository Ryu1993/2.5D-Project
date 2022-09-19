using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum WeaponType { Sword}
    public WeaponType type;
    public Animator animator;
    public bool superArmor;
    public float motionTime;
    public abstract void WeaponAttack();
}
