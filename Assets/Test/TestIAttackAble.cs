using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIAttackAble : MonoBehaviour,IAttackable
{
 
    public void Attack(IDamageable target)
    {
        target.Hit();
    }
}
