using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    [SerializeField]
    RectTransform mapUIRect;
    [SerializeField]
    RoomIconUI roomIcon;
    [SerializeField]
    float roomIconLength;
    [SerializeField]
    Vector2 mapMove;
    bool isMoving;
    public void CreateRoomIcon(List<MapManager.RoomConnectInfo> allRoomList)
    {
        foreach(MapManager.RoomConnectInfo roomInfo in allRoomList)
        {
            RoomIconUI temp = Instantiate(roomIcon, transform);
            temp.transform.localPosition = new Vector2(roomInfo.rectPosition.x*roomIconLength, roomInfo.rectPosition.y*roomIconLength);
            temp.ActiveIcon(roomInfo);
        }
    }
    public void MapMove(MapManager.GateDirection direction)
    {
        Vector3 temp = new Vector3();
        if(direction==MapManager.GateDirection.Up)
        {
            temp = mapMove * new Vector2(0, -1);           
        }
        if(direction==MapManager.GateDirection.Down)
        {
            temp = mapMove * new Vector2(0, 1);
        }
        if(direction==MapManager.GateDirection.Right)
        {
            temp = mapMove * new Vector2(-1,0);
        }
        if(direction==MapManager.GateDirection.Left)
        {
            temp = mapMove * new Vector2(1, 0);
        }
        if(isMoving)
        {
            isMoving = false;
        }
        StartCoroutine(CoMapMove(temp));
    }

    IEnumerator CoMapMove(Vector3 temp)
    {
        isMoving = true;
        bool xpos = false;
        bool ypos = false;
        Vector3 targetVec = mapUIRect.localPosition + temp;
        while(!xpos||!ypos)
        {
            if(!isMoving)
            {
                break;
            }
            xpos = mapUIRect.localPosition.x < targetVec.x + 3 && mapUIRect.localPosition.x > targetVec.x - 3;
            ypos = mapUIRect.localPosition.y < targetVec.y + 3 && mapUIRect.localPosition.y > targetVec.y - 3;
            mapUIRect.localPosition = Vector3.Lerp(mapUIRect.localPosition, targetVec, Time.deltaTime);
            yield return null;
        }
        mapUIRect.localPosition = targetVec;
        isMoving = false;
    }


}
