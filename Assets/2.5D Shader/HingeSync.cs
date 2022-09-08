using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HingeSync : MonoBehaviour
{
    static readonly int Angle = Shader.PropertyToID("_Angle");

    [SerializeField]
    HingeJoint hinge;
    [SerializeField]
    SpriteRenderer spriteRenderer;



    private void Update()
    {
        float angle = -hinge.angle;
        spriteRenderer.material.SetFloat(Angle, angle);

    }

}
