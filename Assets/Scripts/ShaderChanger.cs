using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderChanger : MonoBehaviour
{
    [SerializeField]
    public Shader targetShader;
    [SerializeField]
    [Header("�������� �־��� �Ÿ�")]
    Vector3 posiiton;
    Material temp;
    MeshRenderer[] renderers;
    SkinnedMeshRenderer[] skinRenderes;
    

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
                material.SetVector("Position", posiiton);
            }
        }
        foreach(SkinnedMeshRenderer skinnedMeshRenderer in skinRenderes)
        {
            foreach(Material material in skinnedMeshRenderer.sharedMaterials)
            {
                material.shader = targetShader;
                material.SetVector("Position", posiiton);
            }

        }

    }


}
