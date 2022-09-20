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
    public abstract void WeaponAttack();


}
