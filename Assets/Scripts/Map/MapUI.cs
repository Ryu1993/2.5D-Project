using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    [SerializeField]
    RoomIconUI roomIcon;
    [SerializeField]
    float roomIconLength;
    public void CreateRoomIcon(List<MapManager.RoomConnectInfo> allRoomList)
    {
        foreach(MapManager.RoomConnectInfo roomInfo in allRoomList)
        {
            RoomIconUI temp = Instantiate(roomIcon, transform);
            temp.transform.localPosition = new Vector2(roomInfo.rectPosition.x*roomIconLength, roomInfo.rectPosition.y*roomIconLength);
            temp.ActiveIcon(roomInfo);
        }
    }
}
