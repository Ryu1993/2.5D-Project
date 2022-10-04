using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;

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
        public IResourceLocation roomLocation;
        public Dictionary<GateDirection, RoomConnectInfo> gate = new Dictionary<GateDirection, RoomConnectInfo>();
        public Vector2 rectPosition = Vector2.zero;
        public bool isClear = false;
        public List<GateDirection> GateCheck(bool isFull)
        {
            var result = new List<GateDirection>();
            foreach (var gateDirection in MapManager.instance.gateDirections) if (gate.ContainsKey(gateDirection)==isFull) result.Add(gateDirection);
            return result;
        }
        public void RoomDataSet(RoomData _room) => room = _room;


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
    public Player player;

    #region RoomConnectSetting
    public void RoomCreate(MapInfo mapInfo)
    {
        RoomListCreate(mapInfo);
        while(checkList.Count > 0)
        {
            RoomConnectInfo curRoom = connectedList[Random.Range(0, connectedList.Count)];
            var curRoomNullGate = curRoom.GateCheck(false);
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
        allRoomList.Remove(startInfo);
        while(true)
        {
            RoomConnectInfo randomRoom = connectedList[Random.Range(0, connectedList.Count)];
            if (randomRoom.gate.ContainsKey(GateDirection.Up)) continue;
            if (!SearchInfofromRectPosition(RectPositionFromDirection(GateDirection.Down, randomRoom))) continue;
            randomRoom.gate.Add(GateDirection.Up, endInfo);
            endInfo.gate.Add(GateDirection.Down, randomRoom);
            endInfo.rectPosition = RectPositionFromDirection(GateDirection.Down, randomRoom);
            break;
        }
        for (int i = 0; i < allRoomList.Count; i++) allRoomList[i].RoomDataSet(mapInfo.progressRooms[i]);
        startInfo.RoomDataSet(mapInfo.startRoom);
        endInfo.RoomDataSet(mapInfo.endRoom);
        allRoomList.Add(startInfo);
        allRoomList.Add(endInfo);
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
                if (connectedList.Contains(matchingRoom))
                {
                    if(matchingRoom.rectPosition != RectPositionFromDirection(direction, curRoom)) continue;
                    matchRooms.Add(matchingRoom);
                }
                else
                {
                    if(!SearchInfofromRectPosition(RectPositionFromDirection(direction, curRoom))) continue;
                    matchRooms.Add(matchingRoom);
                }
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
        startInfo = new RoomConnectInfo();
        endInfo = new RoomConnectInfo();
        foreach (RoomData progressRoom in mapInfo.progressRooms) checkList.Add(new RoomConnectInfo());
        foreach (RoomConnectInfo roomConnectInfo in checkList) outputList.Add(roomConnectInfo);
        startInfo.rectPosition = new Vector2(0, 0);
        connectedList.Add(startInfo);
        allRoomList.Add(startInfo);
    }
    private GateDirection MatchDirection(GateDirection direction)
    {
        if (direction == GateDirection.Up) return GateDirection.Down;
        else if (direction == GateDirection.Down) return GateDirection.Up;
        else if (direction == GateDirection.Left) return GateDirection.Right;
        else if (direction == GateDirection.Right) return GateDirection.Left;
        else return GateDirection.Start;
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

    private bool SearchInfofromRectPosition(Vector2 target)
    {
        foreach(var info in allRoomList)
        {
            if (info.rectPosition == target) return false;
        }
        return true;
    }


    #endregion


    public void GameEneter(MapInfo mapInfo)
    {
        RoomCreate(mapInfo);
        mapUI.CreateRoomIcon(allRoomList);
        curRoomInfo = startInfo;
        MapEnter(GateDirection.Start);
    }

    public void MapEnter(GateDirection direction)
    {
        player.PlayerKinematic();
        if (direction != GateDirection.Start)
        {
            curRoomInfo = curRoomInfo.gate[direction]; 
            AddressObject.Release(curRoom);
            mapUI.MapMove(direction);
        }
        if(curRoomInfo.roomLocation == null)
        {
            curRoomInfo.roomLocation = AddressObject.RandomLocation(curRoomInfo.room.map_pack);
        }
        curRoom = Addressables.InstantiateAsync(curRoomInfo.roomLocation).WaitForCompletion();
        if (!curRoom.TryGetComponent<RoomManager>(out curRoomManager))
        {
            curRoom.TryGetComponent<BossRoomManager>(out var bossRoomManager);
            curRoomManager = bossRoomManager;
        }
        curRoomManager = curRoom.GetComponent<RoomManager>();       
        curRoomManager.roomData = curRoomInfo.room;
        curRoomManager.ConnectedSet(curRoomInfo.GateCheck(true));
        if(!curRoomInfo.isClear)curRoomManager.RoomSetting();
        if(curRoomInfo.isClear)curRoomManager.ActivateGate();
        curRoomManager.PlayerSpawn(player.transform, MatchDirection(direction));
        player.PlayerKinematic();
    }

    public void CurMapClear()=> curRoomInfo.isClear = true;








}
