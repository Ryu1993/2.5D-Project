using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using static UnityEditor.FilePathAttribute;

public static class AddressObject
{
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
        return Addressables.InstantiateAsync(location).WaitForCompletion();
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
    public static GameObject GameObjectSet(AssetLabelReference label)
    {
        return Addressables.LoadAssetAsync<GameObject>(label).WaitForCompletion();
    }
    public static List<GameObject> GameObjectsSet(AssetLabelReference label)
    {
        var gameobjects = new List<GameObject>();
        var locations = Locations(label);
        Addressables.LoadAssetsAsync<GameObject>(locations, (go) => gameobjects.Add(go)).WaitForCompletion();
        return gameobjects;
    }

  
    public static List<GameObject> RandomGameObjectsSet(AssetLabelReference label)
    {
        var gameObjects = new List<GameObject>();
        var locations = Locations(label);
        var pickLocations = new List<IResourceLocation>();
        while(locations.Count > 0)
        {
            var location = locations[Random.Range(0, locations.Count)];
            pickLocations.Add(location);
            locations.Remove(location);
        }
        Addressables.LoadAssetsAsync<GameObject>(pickLocations, (go) => gameObjects.Add(go)).WaitForCompletion();
        return gameObjects;
    }




    #region Location
    public static IResourceLocation RandomLocation(AssetLabelReference label)
    {
        var locations = Locations(label);
        return locations[Random.Range(0, locations.Count)];
    }
    public static List<IResourceLocation> RandomLoactions(AssetLabelReference label,int locationNum)
    {
        List<IResourceLocation> resultLocations = new List<IResourceLocation>();
        var locations = Locations(label);
        for(int i = 0; i < locationNum; i++)
        {
            resultLocations.Add(locations[Random.Range(0, locations.Count)]);
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


}
