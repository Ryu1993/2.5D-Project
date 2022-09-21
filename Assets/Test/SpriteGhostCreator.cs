using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGhostCreator : MonoBehaviour
{
    [SerializeField]
    Sprite sprite;
    [SerializeField]
    Material material;
    [SerializeField]
    Transform target;
    bool isSet;


    void GhosteBaseSet(Transform target)
    {
        if(isSet)
        {
            sprite = target.GetComponent<SpriteRenderer>().sprite;
            material = target.GetComponent<SpriteRenderer>().material;

        }

    }

}
