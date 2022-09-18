using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIAttackAble : MonoBehaviour,IAttackable
{
    
 
    public void Attack(IDamageable target)
    {
        target.DirectHit();
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        IDamageable target = collision.transform.GetComponent<IDamageable>();
        target?.Hit(this, transform.position);
    }

}
