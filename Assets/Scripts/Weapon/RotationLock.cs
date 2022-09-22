using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLock : MonoBehaviour
{
    Quaternion baseRotation;

    private void Awake()
    {
        baseRotation = transform.rotation;
    }

    private void Update()
    {
        transform.rotation = baseRotation;
    }
}
