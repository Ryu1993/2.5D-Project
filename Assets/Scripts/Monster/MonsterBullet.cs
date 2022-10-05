using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class MonsterBullet : MonoBehaviour,IReturnable
{
    private NewObjectPool.PoolInfo home;
    private Monster owner;
    private float speed;
    private float duration;
    private float durationCount = 0;
    private bool isReady;

    private void Update()
    {
        if(isReady)
        {
            durationCount += Time.deltaTime;
            transform.Translate(transform.forward * Time.deltaTime * speed);
            if (durationCount >= duration) Return();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out var player))
        {
            owner.target = player.Hit(owner, transform.position);
            Return();
        }
    }
    public void OwnerSet(Monster _owner,float _speed,float _duraition,Quaternion rotation)
    {
        owner = _owner;
        speed = _speed;
        duration = _duraition;
        transform.rotation = rotation;
        isReady = true;
    }

    public void PoolInfoSet(NewObjectPool.PoolInfo pool) => home = pool;
    public void Return()
    {
        isReady = false;
        durationCount = 0;
        NewObjectPool.instance.Return(this.gameObject, home);
    }
}
