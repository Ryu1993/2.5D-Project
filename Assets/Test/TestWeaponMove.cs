using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeaponMove : MonoBehaviour
{
    [SerializeField]
    Transform axis;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    MeshRenderer target;

    private void Update()
    {
        Horizontal();
    }

    void Horizontal()
    {
        transform.RotateAround(axis.position, axis.up, moveSpeed);
        
    }

}
