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
    public class RoomConnectInfo
    {
        [HideInInspector]
        public RoomData room;
        public Dictionary<GateDirection, RoomConnectInfo> gate = new Dictionary<GateDirection, RoomConnectInfo>();
        public Vector2 rectPosition;
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
        public List<GateDirection> GateCheck()
        {
            List<GateDirection> result = new List<GateDirection>();
            foreach (var gateDirection in MapManager.instance.gateDirections)
            {
                if (gate.ContainsKey(gateDirection))
                {
                    result.Add(gateDirection);
                }
            }
            return result;
        }
        public void SetRectPosition(Vector2 vec)
        {
            rectPosition = vec;
        }

    }
    [SerializeField]
    MapUI mapUI;
    List<RoomConnectInfo> checkList;
    List<RoomConnectInfo> outputList;
    [SerializeField]
    List<RoomConnectInfo> connectedList;
    List<RoomConnectInfo> allRoomList;
    RoomConnectInfo startInfo;
    RoomConnectInfo endInfo;
    public void RoomCreate(MapInfo mapInfo)
    {
        int num = 0;
        RoomListCreate(mapInfo);
        while(checkList.Count > 0)
        {
            num++;
            Debug.Log(num);
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
            var matchRooms = MatchRoomList(curRoomMatchDirection, curRoom);
            if(matchRooms.Count == 0)
            {
                continue;
            }

            RoomConnectInfo matchRoom = matchRooms[Random.Range(0, matchRooms.Count)];
            curRoom.gate.Add(curRoomDirection, matchRoom);
            matchRoom.gate.Add(curRoomMatchDirection, curRoom);
            matchRoom.SetRectPosition(SetRectPositionFromDirection(curRoomMatchDirection, matchRoom, curRoom));
            if(checkList.Contains(matchRoom))
            {
                checkList.Remove(matchRoom);
            }
            if(!connectedList.Contains(matchRoom))
            {
                connectedList.Add(matchRoom);
            }
        }
        RoomConnectInfo endConnetRoom;
        List<GateDirection> endConnectRoomDirections;
        while (true)
        {
            endConnetRoom = connectedList[Random.Range(0, connectedList.Count)];
            endConnectRoomDirections = endConnetRoom.NullGateCheck();
            if(endConnectRoomDirections != null)
            {
                break;
            }
        }
        var endConnectRoomDirection = endConnectRoomDirections[Random.Range(0, endConnectRoomDirections.Count)];
        var endRoomDirection = MatchDirection(endConnectRoomDirection);
        endConnetRoom.gate.Add(endConnectRoomDirection, endInfo);
        endInfo.gate.Add(endRoomDirection, endConnetRoom);
        endInfo.SetRectPosition(SetRectPositionFromDirection(endRoomDirection, endInfo, endConnetRoom));
    }
    private List<RoomConnectInfo> MatchRoomList(GateDirection direction, RoomConnectInfo curRoom)
    {
        List<RoomConnectInfo> matchRooms = new List<RoomConnectInfo>();
        foreach (RoomConnectInfo matchingRoom in outputList)
        {
            if (!matchingRoom.gate.ContainsKey(direction))
            {
                if(connectedList.Contains(matchingRoom))
                {
                    if(matchingRoom.rectPosition != SetRectPositionFromDirection(direction, matchingRoom, curRoom))
                    {
                        continue;
                    }
                }
                matchRooms.Add(matchingRoom);
            }
        }
        return matchRooms;
    }
    private void RoomListCreate(MapInfo mapInfo)
    {
        checkList = new List<RoomConnectInfo>();
        outputList = new List<RoomConnectInfo>();
        connectedList = new List<RoomConnectInfo>();
        allRoomList = new List<RoomConnectInfo>();
        startInfo = new RoomConnectInfo(mapInfo.startRoom);
        endInfo = new RoomConnectInfo(mapInfo.endRoom);
        foreach (RoomData progressRoom in mapInfo.progressRooms)
        {
            checkList.Add(new RoomConnectInfo(progressRoom));
        }
        foreach (RoomConnectInfo roomConnectInfo in checkList)
        {
            outputList.Add(roomConnectInfo);
            allRoomList.Add(roomConnectInfo);
        }
        startInfo.SetRectPosition(new Vector2(0,0));
        connectedList.Add(startInfo);
        allRoomList.Add(endInfo);
        allRoomList.Add(startInfo);

    }
    private GateDirection MatchDirection(GateDirection direction)
    {
        if (direction == GateDirection.Up)
        {
            return GateDirection.Down;
        }
        else if (direction == GateDirection.Down)
        {
            return GateDirection.Up;
        }
        else if (direction == GateDirection.Left)
        {
            return GateDirection.Right;
        }
        else
        {
            return GateDirection.Left;
        }
    }
    private Vector2 SetRectPositionFromDirection(GateDirection direction, RoomConnectInfo roomInfo,RoomConnectInfo curRoomInfo)
    {
        float x = curRoomInfo.rectPosition.x;
        float y = curRoomInfo.rectPosition.y;
        if (direction == GateDirection.Up)
        {
            return new Vector2(x, y-1);
        }
        else if (direction == GateDirection.Down)
        {
            return new Vector2(x, y+1);
        }
        else if (direction == GateDirection.Left)
        {
            return new Vector2(x+1, y);
        }
        else
        {
            return new Vector2(x-1, y);
        }
    }

    private void MapUIIconCreate()
    {
        mapUI.CreateRoomIcon(allRoomList);
    }

    [SerializeField]
    MapInfo map;
    void Test()
    {
        RoomCreate(map);
        MapUIIconCreate();
    }
    private void Start()
    {
        Test();
    }


}
