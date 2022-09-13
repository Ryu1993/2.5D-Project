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
    static Queue<GameObject> objQueue;

    public static List<GameObject> Instinates(AssetLabelReference label)
    {
        IList<IResourceLocation> locations = new List<IResourceLocation>();
        objects = new List<GameObject>();
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

    public static List<GameObject> LimitInstinates(AssetLabelReference label, int limit)
    {
        IList<IResourceLocation> locations = new List<IResourceLocation>();
        objects = new List<GameObject>();
        Addressables.LoadResourceLocationsAsync(label.labelString).Completed += (handle) =>
        {
            locations = handle.Result;
        };
        Queue<IResourceLocation> locationQueue = new Queue<IResourceLocation>(locations);
        for (int i = 0; i < limit; i++)
        {
            if (locations.Count != 0)
            {
                objects.Add(Addressables.InstantiateAsync(locationQueue.Dequeue()).Result);
            }
        }
        return objects;
    }

    public static List<GameObject> LimitRandomInstinates(AssetLabelReference label,int limit)
    {
        IList<IResourceLocation> locations = new List<IResourceLocation>();
        objects = new List<GameObject>();
        Addressables.LoadResourceLocationsAsync(label.labelString).Completed += (handle) =>
        {
            locations = handle.Result;
        };
        for (int i = 0; i < limit; i++)
        {
            objects.Add(Addressables.InstantiateAsync(locations[Random.Range(0, locations.Count)]).Result);
        }
        return objects;
    }


    public static int LabelCount(AssetLabelReference label)
    {
        int count = 0;
        Addressables.LoadResourceLocationsAsync(label.labelString).Completed += (handle) =>
        {
            var location = handle.Result;
            if (location != null)
            {
                count = location.Count;
            }
        };
        return count;
    }


    public static GameObject RandonInstinate(AssetLabelReference label)
    {
        Addressables.LoadResourceLocationsAsync(label.labelString).Completed += (handle) =>
        {
            var location = handle.Result;
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
