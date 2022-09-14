using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="MapData/MapInfo")]
public class MapInfo : ScriptableObject
{
    public RoomData startRoom;
    public RoomData[] progressRooms;
    public RoomData endRoom;

}
