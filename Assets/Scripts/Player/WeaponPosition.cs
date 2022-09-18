using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPosition : MonoBehaviour
{
    [SerializeField]
    Transform mousePointer;
    Vector3 mouseVec;
    private void Update()
    {
        TraceMousePointer();
    }
    void TraceMousePointer()
    {
        mouseVec = new Vector3(mousePointer.position.x,0f,mousePointer.position.z);
        transform.LookAt(mouseVec);
    }
    
  
}
