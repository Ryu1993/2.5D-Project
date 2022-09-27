using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MousePointer : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    [SerializeField]
    LayerMask mask;

    private void Update()
    {
        TraceMouse();
    }

    void TraceMouse()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.red);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {
            transform.position = hit.point;
        }
    }
}
