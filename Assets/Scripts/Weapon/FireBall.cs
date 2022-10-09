using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : Weapon
{
    [SerializeField]
    private ParticleSystem particle;
  
    public override void WeaponAttack(int combo)
    {
        for(int i = 0; i < colliders.Length; i++)
        {
            colliders[i] = null;
        }
        particle.Stop();
        particle.Play();
    }

    public void OnParticleCollision(GameObject other)
    {
        Physics.OverlapSphereNonAlloc(other.transform.position, attackRange, colliders, layerMask);
        foreach(Collider collider in colliders)
        {
            if(collider!=null)
            {
                collider.TryGetComponent(out IDamageable target);
                target?.Hit(this, other.transform.position);
            }
        }
    }


}
