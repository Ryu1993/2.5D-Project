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
    public void OnClick()
    {
        StartCoroutine(CoOnClick());
    }
    IEnumerator CoOnClick()
    {
        var operation = Addressables.LoadAssetAsync<ScriptableObject>(AddressObject.RandomLocation(label));
        while(!operation.IsDone)
        {
            yield return null;
        }
        Item temp = operation.Result as Item;
        ItemManager.instance.CreateSceneItem(temp, new Vector3(1, 1, 1));
        Addressables.Release(temp);
    }


  

}
