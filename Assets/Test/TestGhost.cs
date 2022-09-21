using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGhost : MonoBehaviour
{
    [SerializeField]
    Mesh mesh;
    [SerializeField]
    Material material;
    [SerializeField]
    GameObject ghost;
    [SerializeField]
    Transform target;
    [Range(0.0f, 1.0f)]
    [SerializeField]
    float interval;
    WaitForSeconds intervalDelay;
    NewObjectPool.PoolInfo poolInfo;
    

    private void Awake()
    {
        intervalDelay = new WaitForSeconds(interval);
    }

    private void OnEnable()
    {
        poolInfo = NewObjectPool.instance.PoolInfoSet(ghost, 10, 5);
        StartCoroutine(CoCreateGhost());
    }
    IEnumerator CoCreateGhost()
    {
        while (true)
        {
            Transform go = NewObjectPool.instance.Call(poolInfo, target.position);
            go.rotation = target.rotation;
            go.GetComponent<MeshFilter>().sharedMesh = mesh;
            go.GetComponent<MeshRenderer>().material = material;
            yield return intervalDelay;
        }
    }
}
