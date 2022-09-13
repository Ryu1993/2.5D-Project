using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

public static class AddressObject
{
    static List<GameObject> objects = new List<GameObject>();
    static GameObject obj;

    public static List<GameObject> Instinates(AssetLabelReference label)
    {
        IList<IResourceLocation> locations = new List<IResourceLocation>();

        Addressables.LoadResourceLocationsAsync(label.labelString).Completed += (handle) =>
        {
            locations = handle.Result;
        };
        foreach (var location in locations)
        {
            objects.Add(Addressables.InstantiateAsync(location).Result);
        }
        return objects;
    }

    public static GameObject RandonInstiante(AssetLabelReference label)
    {
        IList<IResourceLocation> location = new List<IResourceLocation>();
        Addressables.LoadResourceLocationsAsync(label.labelString).Completed += (handle) =>
        {
            location = handle.Result;
            obj = Addressables.InstantiateAsync(location[Random.Range(0, location.Count)]).Result;
        };
        return obj;
    }


    public static GameObject Instinate(AssetLabelReference label)
    {
        return Addressables.InstantiateAsync(label.labelString).Result;
    }


    public static void Release(GameObject obj)
    {
        Addressables.Release(obj);
    }


}
