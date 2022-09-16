using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    public enum DotType {Burn,Posion };
    public enum CCType { Sturn,Slow};
    public void Attack(IDamageable target);
}
