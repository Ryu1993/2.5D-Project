using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour
{
    [SerializeField]
    DirectionCircle directionCircle;
    [SerializeField]
    Transform target;

    void Update()
    {
        target.localRotation = directionCircle.transform.localRotation;
        transform.rotation = target.rotation;
    }
}
