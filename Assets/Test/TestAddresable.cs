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
    [SerializeField]
    SceneItem sceneItem;
    NewObjectPool.PoolInfo poolInfo;
 
    


    public void OnClick()
    {
        StartCoroutine(CoOnClick());
    }
    IEnumerator CoOnClick()
    {
        if (poolInfo == null) poolInfo = NewObjectPool.instance.PoolInfoSet(sceneItem.gameObject, 3, 2);
        var operation = Addressables.LoadAssetAsync<ScriptableObject>(AddressObject.RandomLocation(label));
        while(!operation.IsDone)
        {
            yield return null;
        }
        Item temp = operation.Result as Item;
        SceneItem scenetest = null;
        if (temp != null)
        { 
            scenetest = NewObjectPool.instance.Call(poolInfo, new Vector3(1, 1, 1)).GetComponent<SceneItem>();
            scenetest.SceneItemSet(temp);
        }
        Addressables.Release(temp);
    }


  

}
