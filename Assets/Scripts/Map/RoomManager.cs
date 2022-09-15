using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Linq;
using UnityEngine.SocialPlatforms.GameCenter;

public class RoomManager : MonoBehaviour
{
    enum Object {Enemy,Npc,Reward}
    public RoomData roomData;
    [SerializeField] Transform enemyPointer;
    [SerializeField] Transform npcPointer;
    [SerializeField] Transform rewardPointer;
    [SerializeField] Transform upGate;
    [SerializeField] Transform downGate;
    [SerializeField] Transform leftGate;
    [SerializeField] Transform rightGate;
    [SerializeField] Transform center;
    Dictionary<Object, List<Transform>> SpawnPoint = new Dictionary<Object, List<Transform>>();
    Dictionary<MapManager.GateDirection,Transform> Gate = new Dictionary<MapManager.GateDirection,Transform>();
    List<MapManager.GateDirection> connectedDirection = new List<MapManager.GateDirection>();
    int monsterCounter = 0;

    private void Awake()
    {
        CreateSpawnPoint();
        CreateGate();
    }
    private void CreateSpawnPoint()
    {
        SpawnPoint.Clear();
        SpawnPoint.Add(Object.Enemy, CreateChildList(enemyPointer));
        SpawnPoint.Add(Object.Npc, CreateChildList(npcPointer));
        SpawnPoint.Add(Object.Reward,CreateChildList(rewardPointer));
    }
    private void CreateGate()
    {
        Gate.Clear();
        Gate.Add(MapManager.GateDirection.Up, upGate);
        Gate.Add(MapManager.GateDirection.Down, downGate);
        Gate.Add(MapManager.GateDirection.Left, leftGate);
        Gate.Add(MapManager.GateDirection.Right, rightGate);
        foreach(MapManager.GateDirection direction in MapManager.instance.gateDirections)
        {
            Gate[direction].GetComponent<Gate>().DirectionSet(direction);
            Gate[direction].gameObject.SetActive(false);
        }
    }
    public void ActivateGate()
    {
        foreach (MapManager.GateDirection direction in connectedDirection)
        {
            Gate[direction].gameObject.SetActive(true);
        }
    }

    public void ConnectedSet(List<MapManager.GateDirection> directions)
    {
        connectedDirection = directions;
    }
    public void RoomSetting()
    {
        if (roomData.curRoomType == RoomData.RoomType.Event)
        {
            NPCSpawn();
        }
        if(roomData.curRoomType == RoomData.RoomType.Battle)
        {
            EnemySpawn();
        }
        if(roomData.curRoomType == RoomData.RoomType.Boss)
        {
            EnemySpawn();
        }
        if(roomData.curRoomType == RoomData.RoomType.Reward)
        {
            RewardSpawn();
        }
    }
    public void PlayerSpawn(Transform player,MapManager.GateDirection direction)
    {
        if(direction == MapManager.GateDirection.Start)
        {
            player.position = center.position;
            return;
        }
        player.position = center.position;
        //player.position = Gate[direction].position;
    }
    private void EnemySpawn()
    {
        monsterCounter = SpawnPoint[Object.Enemy].Count;
        if (monsterCounter > 0)
        {
            foreach (Transform enemy in SpawnPoint[Object.Enemy])
            {
                GameObject tempObj = AddressObject.RandomInstinate(roomData.monster_pack);
                
                Monster temp = tempObj.GetComponent<Monster>();
                temp.transform.position = enemy.position;
                temp.dieEvent += SubMonsterCountEvent;
            }
        }
    }
    private void SubMonsterCountEvent()
    {
        monsterCounter--;
        if(monsterCounter == 0)
        {
            RewardSpawn();
        }
    }
    private void NPCSpawn()
    {
        int count = SpawnPoint[Object.Npc].Count;
        if (count > 0)
        {
            List<GameObject> npc = AddressObject.LimitInstinates(roomData.npc_pack, count);
            for (int i = 0; i < npc.Count; i++)
            {
                npc[i].transform.position = SpawnPoint[Object.Npc][i].position;
            }
        }
        ActivateGate();
    }
    private void RewardSpawn()
    {
        //List<GameObject> rewards = AddressObject.LimitRandomInstinates(roomData.reward_pack, SpawnPoint[Object.Reward].Count);
        //for(int i = 0; i < rewards.Count; i++)
        //{
        //    rewards[i].transform.position = SpawnPoint[Object.Reward][i].position;
        //}
        ActivateGate();
    }

    private List<Transform> CreateChildList(Transform transform)
    {
        List<Transform> childList = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            childList.Add(transform.GetChild(i));
        }
        return childList;
    }


}
