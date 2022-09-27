using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.Arm;

public class GhostCreator : MonoBehaviour
{
    private NewObjectPool.PoolInfo poolInfo; 
    [SerializeField]
    private GhostObject ghost;
    [SerializeField]
    private SkinnedMeshRenderer[] skmrList;
    [SerializeField, Header("IllusionDuration"),Range(0,1f)]
    private float duration;
    [SerializeField]
    private float cycle;
    private float count = 0f;
    private bool isPlay;
    [SerializeField]
    private Gradient original;
    private List<Mesh> bakedMesh;
    private List<Vector3> meshPosition;
    private List<Quaternion> meshRotation;

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => NewObjectPool.instance != null);
        poolInfo = NewObjectPool.instance.PoolInfoSet(ghost.gameObject, 8, 4);
    }

    private void Update()
    {
        if (isPlay)CreateGhost();
    }

    public void Swith() => isPlay = !isPlay;
    private void CreateGhost()
    {
        count += Time.deltaTime;
        if(count>cycle)
        {
            count = 0;
            MeshBake();
            GhostObject tempGhost = NewObjectPool.instance.Call(poolInfo).GetComponent<GhostObject>();
            tempGhost.SetGhost(bakedMesh, meshPosition, meshRotation, original, duration);
        }
    }
    private void MeshBake()
    {
        bakedMesh = new List<Mesh>();
        meshPosition = new List<Vector3>();
        meshRotation = new List<Quaternion>();
        for (int i=0;i<skmrList.Length;i++)
        {
            bakedMesh.Add(new Mesh());
            Vector3 position = skmrList[i].transform.position;
            Quaternion rotation = skmrList[i].transform.rotation;
            skmrList[i].BakeMesh(bakedMesh[i]);
            meshPosition.Add(position);
            meshRotation.Add(rotation);
        }
    }




}
