using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : Singleton<MapManager>
{
    [HideInInspector]
    public enum GateDirection { Up, Down,Left,Right,Start }
    [HideInInspector]
    public GateDirection[] gateDirections = {GateDirection.Up,GateDirection.Down,GateDirection.Left,GateDirection.Right};
    [SerializeField]
    MapUI mapUI;
    public class RoomConnectInfo
    {
        [HideInInspector]
        public RoomData room;
        public Dictionary<GateDirection, RoomConnectInfo> gate = new Dictionary<GateDirection, RoomConnectInfo>();
        public Vector2 rectPosition;
        public bool isClear = false;
        public RoomConnectInfo(RoomData _room)
        {
            room = _room;
        }
        public List<GateDirection> NullGateCheck()
        {
            var result = new List<GateDirection>();
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
            var result = new List<GateDirection>();
            foreach (var gateDirection in MapManager.instance.gateDirections)
            {
                if (gate.ContainsKey(gateDirection))
                {
                    result.Add(gateDirection);
                }
            }
            return result;
        }

    }
    List<RoomConnectInfo> checkList;
    List<RoomConnectInfo> outputList;
    List<RoomConnectInfo> connectedList;
    List<RoomConnectInfo> allRoomList;
    RoomConnectInfo startInfo;
    RoomConnectInfo endInfo;
    RoomConnectInfo curRoomInfo;
    RoomManager curRoomManager;
    GameObject curRoom;
    Transform player;

    #region RoomConnectSetting
    public void RoomCreate(MapInfo mapInfo)
    {
        RoomListCreate(mapInfo);
        while(checkList.Count > 0)
        {
            RoomConnectInfo curRoom = connectedList[Random.Range(0, connectedList.Count)];
            var curRoomNullGate = curRoom.NullGateCheck();
            if (curRoomNullGate.Count == 0){ RemoveList(curRoom); continue; }
            GateDirection curRoomDirection = curRoomNullGate[Random.Range(0, curRoomNullGate.Count)];
            GateDirection curRoomMatchDirection = MatchDirection(curRoomDirection);
            var matchRooms = MatchRoomList(curRoomMatchDirection, curRoom);
            if (matchRooms.Count == 0) continue;
            RoomConnectInfo matchRoom = matchRooms[Random.Range(0, matchRooms.Count)];
            curRoom.gate.Add(curRoomDirection, matchRoom);
            matchRoom.gate.Add(curRoomMatchDirection, curRoom);
            matchRoom.rectPosition = RectPositionFromDirection(curRoomMatchDirection, curRoom);
            if(checkList.Contains(matchRoom)) checkList.Remove(matchRoom);
            if(!connectedList.Contains(matchRoom)) connectedList.Add(matchRoom);
            if(!allRoomList.Contains(matchRoom)) allRoomList.Add(matchRoom);
        }
        allRoomList.Add(startInfo);
        while (true)
        {
            RoomConnectInfo endConnetRoom = connectedList[Random.Range(0, connectedList.Count)];
            var endConnectRoomDirections = endConnetRoom.NullGateCheck();
            if (endConnectRoomDirections.Count == 0) { connectedList.Remove(endConnetRoom); continue; }
            var endConnectRoomDirection = endConnectRoomDirections[Random.Range(0, endConnectRoomDirections.Count)];
            var endRoomDirection = MatchDirection(endConnectRoomDirection);
            Vector2 endRoomPosition = RectPositionFromDirection(endRoomDirection, endConnetRoom);
            if (!CheckRectPosition(endRoomPosition)) continue;
            endConnetRoom.gate.Add(endConnectRoomDirection, endInfo);
            endInfo.gate.Add(endRoomDirection, endConnetRoom);
            endInfo.rectPosition = endRoomPosition;
            allRoomList.Add(endInfo);
            break;
        }
    }
    private void RemoveList(RoomConnectInfo curRoom)
    {
        connectedList.Remove(curRoom);
        outputList.Remove(curRoom);
        checkList.Remove(curRoom);
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
                    if(matchingRoom.rectPosition != RectPositionFromDirection(direction, curRoom))
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
        foreach (RoomData progressRoom in mapInfo.progressRooms) checkList.Add(new RoomConnectInfo(progressRoom));
        foreach (RoomConnectInfo roomConnectInfo in checkList) outputList.Add(roomConnectInfo);
        startInfo.rectPosition = new Vector2(0, 0);
        connectedList.Add(startInfo);
    }
    private GateDirection MatchDirection(GateDirection direction)
    {
        if (direction == GateDirection.Up) return GateDirection.Down;
        else if (direction == GateDirection.Down) return GateDirection.Up;
        else if (direction == GateDirection.Left) return GateDirection.Right;
        else return GateDirection.Left;
    }
    private Vector2 RectPositionFromDirection(GateDirection direction,RoomConnectInfo curRoomInfo)
    {
        float x = curRoomInfo.rectPosition.x;
        float y = curRoomInfo.rectPosition.y;
        if (direction == GateDirection.Up) return new Vector2(x, y - 1);
        else if (direction == GateDirection.Down) return new Vector2(x, y + 1);
        else if (direction == GateDirection.Left) return new Vector2(x + 1, y);
        else return new Vector2(x - 1, y);
    }
    private bool CheckRectPosition(Vector2 vec)
    {
        foreach(RoomConnectInfo room in allRoomList)
        {
            if(room.rectPosition == vec)
            {
                return false;
            }
        }
        return true;

    }
    #endregion


    private void MapUIIconCreate()
    {
        mapUI.CreateRoomIcon(allRoomList);
    }


    public void GameEneter(MapInfo mapInfo)
    {
        player = Player.instance.transform;
        RoomCreate(mapInfo);
        curRoomInfo = startInfo;
        MapEnter(GateDirection.Start);
    }

    public void MapEnter(GateDirection direction)
    {
        Player.instance.PlayerKinematic();
        if (direction != GateDirection.Start)
        {
            curRoomInfo.isClear = true;
            curRoomInfo = curRoomInfo.gate[direction]; 
            AddressObject.Release(curRoom);
        }
        curRoom = AddressObject.RandomInstinate(curRoomInfo.room.map_pack);
        curRoomManager = curRoom.GetComponent<RoomManager>();
        curRoomManager.roomData = curRoomInfo.room;
        curRoomManager.ConnectedSet(curRoomInfo.GateCheck());
        if(!curRoomInfo.isClear)curRoomManager.RoomSetting();
        if(curRoomInfo.isClear)curRoomManager.ActivateGate();
        curRoomManager.PlayerSpawn(player, MatchDirection(direction));
        Player.instance.PlayerKinematic();
    }








    [SerializeField]
    MapInfo map;
    void Test()
    {
        GameEneter(map);
    }
    private void Start()
    {
        Test();
    }


}
