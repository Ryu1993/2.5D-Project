using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TestAddresable : MonoBehaviour
{
    [SerializeField]
    AssetLabelReference label;
    [SerializeField]
    List<GameObject> testList = new List<GameObject>();
    AsyncOperationHandle<IList<GameObject>> operationHandle;


    private void Start()
    {
        operationHandle = AddressObject.RandomGameObjectHandlesSet(label, testList,1);
        operationHandle.WaitForCompletion();
        StartCoroutine(ListClear());
    }
    IEnumerator ListClear()
    {
        yield return new WaitForSeconds(3);
        testList.Clear();
        Addressables.Release(operationHandle);
    }

}
