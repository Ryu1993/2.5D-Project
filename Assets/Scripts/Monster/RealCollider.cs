using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealCollider : MonoBehaviour
{
    [SerializeField]
    Monster monster;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamageable>(out var target)) monster.target = target.Hit(monster, transform.position);
    }
}
