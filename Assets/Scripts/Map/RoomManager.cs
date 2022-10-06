using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Linq;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;
using static MapManager;

public class RoomManager : MonoBehaviour
{
    public enum Object {Enemy,Npc,Reward}
    public RoomData roomData;
    [SerializeField] protected Transform enemyPointer;
    [SerializeField] protected Transform npcPointer;
    [SerializeField] protected Transform rewardPointer;
    [SerializeField] protected Transform gate;
    [SerializeField] protected Transform center;
    protected Dictionary<Object, List<Transform>> SpawnPoint = new Dictionary<Object, List<Transform>>();
    protected Dictionary<GateDirection,Transform> Gate = new Dictionary<GateDirection,Transform>();
    protected List<GateDirection> connectedDirection = new List<GateDirection>();
    protected List<NewObjectPool.PoolInfo> enemyPools = new List<NewObjectPool.PoolInfo>();
    protected List<GameObject> enemys = new List<GameObject>();
    protected List<GameObject> npcs = new List<GameObject>();
    protected List<RewardChest> rewards = new List<RewardChest>();
    protected UnityAction chestOpen;
    protected UnityAction npcInteraction;
    protected int monsterCounter = 0;

    protected void Awake()
    {
        CreateSpawnPoint();
        CreateGate();
    }
    protected void CreateSpawnPoint()
    {
        SpawnPoint.Clear();
        SpawnPoint.Add(Object.Enemy, CreateChildList(enemyPointer));
        SpawnPoint.Add(Object.Npc, CreateChildList(npcPointer));
        SpawnPoint.Add(Object.Reward,CreateChildList(rewardPointer));
    }
    protected void CreateGate()
    {
        Gate.Clear();
        foreach(GateDirection direction in MapManager.instance.gateDirections)
        {
            Gate.Add(direction, gate.Find(direction.ToString()));
            if (Gate[direction] != null)
            {
                Gate[direction].TryGetComponent<Gate>(out var gate);
                gate.DirectionSet(direction);
            }
        }
    }
    public void ActivateGate()
    {
        foreach (GateDirection direction in connectedDirection)
        {
            Gate[direction].GetComponent<Gate>().GateOpen();
        }
    }
    public void ConnectedGateSet(List<GateDirection> directions) => connectedDirection = directions;
    public void RoomSetting()
    { 
        if (roomData.curRoomType == RoomData.RoomType.Event) NPCSpawn();
        if (roomData.curRoomType == RoomData.RoomType.Battle) EnemySpawn();
        if (roomData.curRoomType == RoomData.RoomType.Boss) BossSpawn();
        if (roomData.curRoomType == RoomData.RoomType.Reward) RewardSpawn();
    }
    public void PlayerSpawn(Transform player,MapManager.GateDirection direction)
    {
        if(direction == MapManager.GateDirection.Start) player.position = center.position;
        else player.position = Gate[direction].transform.GetChild(0).position;
    }
    protected void EnemySpawn()
    {
        monsterCounter = SpawnPoint[Object.Enemy].Count;    
        AsyncOperationHandle<IList<GameObject>> list = AddressObject.GameObjectHandlesSet(roomData.monster_pack, enemys);
        list.WaitForCompletion();
        if (monsterCounter > 0)
        {
            enemyPools.Clear();
            foreach (GameObject enemy in enemys)
            {
                enemyPools.Add(NewObjectPool.instance.PoolInfoSet(enemy, 6, 3));
            }
            Debug.Log(enemyPools.Count);
            foreach (Transform enemyPostion in SpawnPoint[Object.Enemy])
            {
                var monster = NewObjectPool.instance.Call(enemyPools[Random.Range(0, enemyPools.Count)], enemyPostion.position).GetComponent<Monster>();
                monster.deadEvent -= SubMonsterCountEvent;
                monster.deadEvent += SubMonsterCountEvent;
            }          
        }
        Addressables.Release(list);
    }

    protected void SubMonsterCountEvent()
    {
        monsterCounter--;
        if (monsterCounter == 0) RewardSpawn();
    }
    public void EnemyAdd(int num)
    {
        int subCount = 0;
        for (int i = 0; i < num; i++)
        {
            subCount++;
            if (subCount < monsterCounter)
            {
                var pool = enemyPools[Random.Range(0, enemyPools.Count)];
                var monster = NewObjectPool.instance.Call(pool).GetComponent<Monster>();
                monster.deadEvent += SubMonsterCountEvent;
                monster.PoolInfoSet(pool);
            }
        }

     }

    protected virtual void BossSpawn()
    {
    }


    protected void NPCSpawn()
    {
        if(roomData.npc_pack!=null)
        {
            int count = SpawnPoint[Object.Npc].Count;
            if (count > 0)
            {
                npcs = AddressObject.LimitInstinates(roomData.npc_pack, count);
                IEventable[] eventables = new IEventable[npcs.Count];
                for (int i = 0; i < npcs.Count; i++)
                {
                    npcs[i].transform.position = SpawnPoint[Object.Npc][i].position;
                    if(npcs[i].TryGetComponent<IEventable>(out var npc))
                    {
                        npcInteraction += npc.EventClear;
                        eventables[i] = npc;
                    }
                }
                npcInteraction += MapManager.instance.CurMapClear;
                foreach(var eventable in eventables)
                {
                    eventable.EventInteraction(npcInteraction);
                }
            }
        }
        ActivateGate();
    }
    protected void RewardSpawn()
    {
        if (roomData.isReward)
        {
            List<GameObject> rewardGo = AddressObject.LimitRandomInstinates(roomData.reward_pack, SpawnPoint[Object.Reward].Count);
            foreach (var go in rewardGo)
            {
                if (go.TryGetComponent<RewardChest>(out var reward))
                {
                    rewards.Add(reward);
                    chestOpen += reward.TriggerOn;
                }
            }
            for (int i = 0; i < rewards.Count; i++)
            {
                rewards[i].transform.position = SpawnPoint[Object.Reward][i].position;
                rewards[i].openEvent = chestOpen;
            }
        }
        else MapManager.instance.CurMapClear();
        ActivateGate();
    }

    protected List<Transform> CreateChildList(Transform transform)
    {
        List<Transform> childList = new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            childList.Add(transform.GetChild(i));
        }
        return childList;
    }

    protected void OnDestroy()
    {
        foreach(GameObject go in npcs)
        {
            if(go!=null) Addressables.Release(go);

        }
        foreach(RewardChest go in rewards)
        {
            if (go != null) Addressables.Release(go.gameObject);
        }
    }



}
