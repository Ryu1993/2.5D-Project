using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour,IAttackable
{
    [HideInInspector]
    public Player player;
    [SerializeField]
    protected LayerMask layerMask;
    public bool superArmor;
    public float damage;
    public float afterDelay;
    public float attackRange;
    public int maxCombo;
    protected Collider[] colliders = new Collider[8];
    protected float realDamage;



    public abstract void WeaponAttack(int combo);
    public virtual void Attack(IDamageable target)
    {
        if (realDamage == 0) realDamage = damage;
        target.DirectHit(realDamage);
    }


}
