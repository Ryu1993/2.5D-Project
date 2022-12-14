using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionCircle : MonoBehaviour
{
    [SerializeField]
    Transform mousePointer;
    Vector3 mouseVec;
    public bool isStop;


    private void FixedUpdate() =>TraceMousePointer();

    void TraceMousePointer()
    {
        if(!isStop)
        {
            mouseVec = new Vector3(mousePointer.position.x, transform.position.y, mousePointer.position.z);
            transform.LookAt(mouseVec);
        }
    }
    
  
}
