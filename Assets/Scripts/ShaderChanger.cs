using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderChanger : MonoBehaviour
{
    [SerializeField]
    public Shader targetShader;
    [SerializeField]
    [Header("원본부터 멀어질 거리")]
    Vector3 posiiton;
    Material temp;
    [SerializeField]
    MeshRenderer[] renderers;
    [SerializeField]
    SkinnedMeshRenderer[] skinRenderes;

    static readonly int Position = Shader.PropertyToID("_Position");

    public void SearchRenderer()
    {
        renderers = transform.GetComponentsInChildren<MeshRenderer>();
        skinRenderes = transform.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public void ChangeShader()
    {
        foreach (MeshRenderer meshRenderer in renderers)
        {
            foreach(Material material in meshRenderer.sharedMaterials)
            {
                material.shader = targetShader;
                material.SetVector(Position, posiiton);
            }
        }
        foreach(SkinnedMeshRenderer skinnedMeshRenderer in skinRenderes)
        {
            foreach(Material material in skinnedMeshRenderer.sharedMaterials)
            {
                material.shader = targetShader;
                material.SetVector(Position, posiiton);
            }

        }

    }


}
