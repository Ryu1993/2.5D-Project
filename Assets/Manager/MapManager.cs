using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [HideInInspector]
   public enum GateDirection { Up, Down,Left,Right }
    [HideInInspector]
   public GateDirection[] gateDirections = {GateDirection.Up,GateDirection.Down,GateDirection.Left,GateDirection.Right};
    [System.Serializable]
   class RoomConnectInfo
    {
        [HideInInspector]
        public RoomData room;
        public Dictionary<GateDirection, RoomConnectInfo> gate = new Dictionary<GateDirection, RoomConnectInfo>();
        public RoomConnectInfo(RoomData _room)
        {
            room = _room;
        }
        public List<GateDirection> NullGateCheck()
        {
            List<GateDirection> result = new List<GateDirection>();
            foreach(var gateDirection in MapManager.instance.gateDirections)
            {
                if(!gate.ContainsKey(gateDirection))
                {
                    result.Add(gateDirection);
                }
            }
            return result;
        }
    }

    List<RoomConnectInfo> checkList = new List<RoomConnectInfo>();
    List<RoomConnectInfo> outputList = new List<RoomConnectInfo>();
    [SerializeField]
    List<RoomConnectInfo> connectedList = new List<RoomConnectInfo>();
    private void RoomCreate()
    {
        while(checkList.Count > 0)
        {
            RoomConnectInfo curRoom = connectedList[Random.Range(0, connectedList.Count)];
            var curRoomNullGate = curRoom.NullGateCheck();
            if (curRoomNullGate.Count == 0)
            {
                connectedList.Remove(curRoom);
                outputList.Remove(curRoom);
                checkList.Remove(curRoom);
                continue;
            }
            GateDirection curRoomDirection = curRoomNullGate[Random.Range(0, curRoomNullGate.Count)];
            GateDirection curRoomMatchDirection = MatchDirection(curRoomDirection);
            List<RoomConnectInfo> matchRooms = new List<RoomConnectInfo>();
            foreach(RoomConnectInfo matchingRoom in outputList)
            {
                if(!matchingRoom.gate.ContainsKey(curRoomMatchDirection))
                {
                    matchRooms.Add(matchingRoom);
                }
            }
            RoomConnectInfo matchRoom = matchRooms[Random.Range(0, matchRooms.Count)];
            curRoom.gate.Add(curRoomDirection, matchRoom);
            matchRoom.gate.Add(curRoomMatchDirection, curRoom);
            if(checkList.Contains(matchRoom))
            {
                checkList.Remove(matchRoom);
            }
            if(!connectedList.Contains(matchRoom))
            {
                connectedList.Add(matchRoom);
            }
        }
    }

    private GateDirection MatchDirection(GateDirection direction)
    {
        
        if(direction == GateDirection.Up)
        {
            return GateDirection.Down;
        }
        else if(direction==GateDirection.Down)
        {
            return GateDirection.Up;
        }
        else if(direction==GateDirection.Left)
        {
            return GateDirection.Right;
        }
        else
        {
            return GateDirection.Left;
        }
    }

    void RoomListCreate(MapInfo mapInfo)
    {
        RoomConnectInfo startRoom = new RoomConnectInfo(mapInfo.startRoom);
        RoomConnectInfo endRoom = new RoomConnectInfo(mapInfo.endRoom);
        checkList.Add(endRoom);
        connectedList.Add(startRoom);
        foreach (RoomData progressRoom in mapInfo.progressRooms)
        {
            checkList.Add(new RoomConnectInfo(progressRoom));
        }
        foreach (RoomConnectInfo roomConnectInfo in checkList)
        {
            outputList.Add(roomConnectInfo);
        }
    }


    [SerializeField]
    MapInfo map;
    void Test()
    {
        RoomListCreate(map);
        RoomCreate();
    }
    private void Start()
    {
        Test();
    }


}
