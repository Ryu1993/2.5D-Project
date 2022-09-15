using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    MapManager.GateDirection curDirection;

    public void DirectionSet(MapManager.GateDirection direction)
    {
        curDirection = direction;
    }
    private void OnCollisionEnter(Collision collision)
    {
        MapManager.instance.MapEnter(curDirection);
    }


}
