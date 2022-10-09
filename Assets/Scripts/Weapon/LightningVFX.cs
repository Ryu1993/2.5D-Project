using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LightningVFX : MonoBehaviour, IReturnable
{
    private NewObjectPool.PoolInfo home;
    [SerializeField]
    private Transform[] childParticles;
    private ParticleSystem particle;

    private void Awake() => transform.TryGetComponent(out particle);


    private void Update()
    {
        if(particle.isStopped)Return();
    }

    public void ScaleSet(Vector3 vec)
    {
        foreach (var transform in childParticles)
        {
            transform.localScale = vec;
        }
    }

    public void PoolInfoSet(NewObjectPool.PoolInfo pool) => home = pool;

    public void Return()
    {
        ScaleSet(Vector3.one);
        NewObjectPool.instance.Return(this.gameObject, home);
    }

}
