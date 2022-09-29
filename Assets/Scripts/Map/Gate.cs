using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    MapManager.GateDirection curDirection;
    bool isOpen;
    [SerializeField]
    Transform gateParticle;
    [SerializeField]
    BoxCollider colli;

    public void DirectionSet(MapManager.GateDirection direction)
    {
        gateParticle.gameObject.SetActive(true);
        curDirection = direction;
    }
    public void GateOpen()
    {
        gateParticle.gameObject.SetActive(false);
        colli.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        MapManager.instance.MapEnter(curDirection);
    }
    

}
