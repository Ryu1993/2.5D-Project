using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GhostObject : MonoBehaviour, IReturnable
{
    [SerializeField]
    MeshFilter[] meshFilters;
    [SerializeField]
    MeshRenderer[] meshRenderers;
    Gradient gradient;
    NewObjectPool.PoolInfo poolInfo;
    UnityAction activeBehaviour;
    [SerializeField]
    Material material;
    float duration;
    float count;
    private void FixedUpdate()
    {
        activeBehaviour?.Invoke();
    }
    public void PoolInfoSet(NewObjectPool.PoolInfo pool)=> poolInfo= pool;
    public void Return() => NewObjectPool.instance.Return(this.gameObject, poolInfo);
    public void SetGhost(List<Mesh> _mesh,List<Vector3>_position, List<Quaternion> _rotation,Gradient original,float duration)
    {
        for(int i = 0; i<meshFilters.Length;i++)
        {
            meshFilters[i].sharedMesh = _mesh[i];
            meshRenderers[i].material = material;
            meshFilters[i].transform.position = _position[i];
            meshFilters[i].transform.rotation = _rotation[i];
        }
        gradient = original;
        this.duration = duration;
        activeBehaviour += ActivateGhost;
    }
    private void ActivateGhost()
    {
        count += Time.fixedDeltaTime;
        material.color = gradient.Evaluate(count);
        if(count>=duration)
        {
            count = 0;
            activeBehaviour -= ActivateGhost;
            Return();
        }
    }


}
