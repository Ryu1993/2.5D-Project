using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using static UnityEditor.FilePathAttribute;

public static class AddressObject
{
    public static ScriptableObject RandomInstinateScriptable(AssetLabelReference label)
    {
        var location = RandomLocation(label);
        var scriptableObj = Addressables.LoadAssetAsync<ScriptableObject>(location).WaitForCompletion();
        return scriptableObj;
    }
    public static ScriptableObject InstinateScriptable(AssetLabelReference label)
    {
        return Addressables.LoadAssetAsync<ScriptableObject>(label).WaitForCompletion();
    }

    public static List<ScriptableObject> InstinatesScriptable(AssetLabelReference label)
    {
        var locations = Locations(label);
        List<ScriptableObject> scriptables = new List<ScriptableObject>();
        Addressables.LoadAssetsAsync<ScriptableObject>(locations, (go) => scriptables.Add(go)).WaitForCompletion();
        return scriptables;
    }

    public static List<GameObject> Instinates(AssetLabelReference label)
    {
        List<GameObject> objectList = new List<GameObject>();
        var locations = Locations(label);
        foreach (var location in locations)
        {
            objectList.Add(Addressables.InstantiateAsync(location).WaitForCompletion());
        }
        return objectList;
    } // 라벨에 있는거 전부
    public static List<GameObject> LimitInstinates(AssetLabelReference label, int limit) // 라벨에 있는거 일부만,라벨 전체갯수보다 많을경우 무시
    {
        List<GameObject> objectList = new List<GameObject>();
        var locations = Locations(label);
        for (int i = 0; i < limit; i++)
        {
            if(locations.Count > 0)
            {
                var location = locations[Random.Range(0, locations.Count)];
                objectList.Add(Addressables.InstantiateAsync(location).WaitForCompletion());
                locations.Remove(location);
            }
        }
        return objectList;
    }
    public static List<GameObject> LimitRandomInstinates(AssetLabelReference label,int limit)
    {
        List<GameObject> objectList = new List<GameObject>();
        var locations = RandomLoactions(label,limit);
        foreach (var location in locations)
        {
            objectList.Add(Addressables.InstantiateAsync(location).WaitForCompletion());
        }
        return objectList;
    } // 라벨에 있는거 랜덤하게, 갯수만큼, 중복가능
    public static GameObject RandomInstinate(AssetLabelReference label)
    {
        var location = RandomLocation(label);
        GameObject go = Addressables.InstantiateAsync(location).WaitForCompletion();
        return go;
    } // 라벨에 있는거 랜덤하게 하나 
    public static GameObject Instinate(IResourceLocation location)
    {
        return Addressables.InstantiateAsync(location).WaitForCompletion();
    }
    public static List<GameObject> Instinates(List<IResourceLocation> locations)
    {
        List<GameObject> gameObjects = new List<GameObject>();
        foreach(var location in locations)
        {
            gameObjects.Add(Addressables.InstantiateAsync(location).WaitForCompletion());
        }
        return gameObjects;
    }

    public static AsyncOperationHandle<GameObject> GameObjectHandleSet(AssetLabelReference label)
    {
        return Addressables.LoadAssetAsync<GameObject>(label);
    }
    public static AsyncOperationHandle<IList<GameObject>> GameObjectHandlesSet(AssetLabelReference label,List<GameObject> gameObjects)
    {
        return Addressables.LoadAssetsAsync<GameObject>(label, (go) => gameObjects.Add(go));
    }
    public static AsyncOperationHandle<IList<GameObject>> RandomGameObjectHandlesSet(AssetLabelReference label,List<GameObject> gameObjects,int limit)
    {
        var locations = Locations(label);
        var pickLocations = new List<IResourceLocation>();
        for(int i = 0;i<limit;i++)
        {
            if (locations.Count == 0) continue;
            var location = locations[Random.Range(0, locations.Count)];
            pickLocations.Add(location);
            locations.Remove(location);
        }
        return Addressables.LoadAssetsAsync<GameObject>(pickLocations, (go) => gameObjects.Add(go));
    }
    #region Location
    public static IResourceLocation RandomLocation(AssetLabelReference label)
    {
        var locations = Locations(label);
        var location = locations[Random.Range(0, locations.Count)];
        return location;
    }
    public static List<IResourceLocation> RandomLoactions(AssetLabelReference label,int locationNum)
    {
        List<IResourceLocation> resultLocations = new List<IResourceLocation>();
        var locations = Locations(label);
        for(int i = 0; i < locationNum; i++)
        {
            if(locations.Count != 0)
            {
                var location = locations[Random.Range(0, locations.Count)];
                resultLocations.Add(location);
            }
        }
        return resultLocations;
    }
    public static IList<IResourceLocation> Locations(AssetLabelReference label)
    {
        List<IResourceLocation> locations = new List<IResourceLocation>();
        var Ilocations = Addressables.LoadResourceLocationsAsync(label.labelString).WaitForCompletion();
        foreach(var location in Ilocations)
        {
            locations.Add(location);
        }
        return locations;
    }
    #endregion

    public static GameObject Instinate(AssetLabelReference label)
    {
        return Addressables.InstantiateAsync(label.labelString).WaitForCompletion();
    }
    public static bool Release(GameObject obj)
    {
        return Addressables.ReleaseInstance(obj);
    }
    public static void Release(ScriptableObject obj)
    {
        Addressables.Release<ScriptableObject>(obj);
    }


}
