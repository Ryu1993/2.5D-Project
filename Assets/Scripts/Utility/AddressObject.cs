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
        var locations = Addressables.LoadResourceLocationsAsync(label.labelString).WaitForCompletion();
        foreach (var location in locations)
        {
            objectList.Add(Addressables.InstantiateAsync(location).WaitForCompletion());
        }
        return objectList;
    }

    public static List<GameObject> LimitInstinates(AssetLabelReference label, int limit)
    {
        List<GameObject> objectList = new List<GameObject>();
        var locations = Addressables.LoadResourceLocationsAsync(label.labelString).WaitForCompletion();
        for (int i = 0; i < limit; i++)
        {
            if(locations.Count > 0)
            {
                IResourceLocation location = locations[Random.Range(0, locations.Count)];
                objectList.Add(Addressables.InstantiateAsync(location).WaitForCompletion());
                locations.Remove(location);
            }
        }
        return objectList;
    }

    public static List<GameObject> LimitRandomInstinates(AssetLabelReference label,int limit)
    {
        List<GameObject> objectList = new List<GameObject>();
        var locations = Addressables.LoadResourceLocationsAsync(label.labelString).WaitForCompletion();
        for (int i = 0; i < limit; i++)
        {
            objectList.Add(Addressables.InstantiateAsync(locations[Random.Range(0, locations.Count)]).WaitForCompletion());
        }
        return objectList;
    }


    public static GameObject RandomInstinate(AssetLabelReference label)
    {
        var locations = Addressables.LoadResourceLocationsAsync(label.labelString).WaitForCompletion();
        return Addressables.InstantiateAsync(locations[Random.Range(0, locations.Count)]).WaitForCompletion();
    }




    public static GameObject Instinate(AssetLabelReference label)
    {
        return Addressables.InstantiateAsync(label.labelString).WaitForCompletion();
    }


    public static bool Release(GameObject obj)
    {
        return Addressables.ReleaseInstance(obj);
    }


}
