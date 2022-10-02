using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="StageInfo")]
public class StageInfo : ScriptableObject
{
    public List<MapInfo> progressMap;
    public MapInfo endMap;

}


