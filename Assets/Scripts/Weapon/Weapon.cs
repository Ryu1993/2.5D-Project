using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public enum WeaponType { Sword,Magic}
    [SerializeField]
    public WeaponType type;
    [SerializeField]
    public bool superArmor;
    public Player player;
    [SerializeField]
    public Sprite sprite;
    [SerializeField]
    protected float damage;
    [SerializeField]
    public float afterDelay;
    [SerializeField]
    public float forwardLength;
    public int maxCombo;
    public abstract void WeaponAttack();


}
