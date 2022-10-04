using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum WeaponType { Sword}
    [SerializeField]
    public WeaponType type;
    [SerializeField]
    public bool superArmor;
    public Player player;
    public Transform attackPoint;
    [SerializeField]
    public Sprite sprite;
    [SerializeField]
    protected float damage;
    public int maxCombo;
    public abstract void WeaponAttack();


}
