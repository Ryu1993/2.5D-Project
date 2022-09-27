using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    MapManager.GateDirection curDirection;
    bool isOpen;
    [SerializeField]
    Transform gateParticle;

    public void DirectionSet(MapManager.GateDirection direction)
    {
        gateParticle.gameObject.SetActive(false);
        curDirection = direction;
    }
    public void GateOpen()
    {
        gateParticle.gameObject.SetActive(true);
        isOpen = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isOpen) return;
        MapManager.instance.MapEnter(curDirection);

    }
    

}
