using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TestAddresable : MonoBehaviour
{
    [SerializeField]
    AssetLabelReference label;
    [SerializeField]
    List<GameObject> testList;


    private void Start()
    {
       testList = AddressObject.RandomGameObjectsSet(label);
    }


}
