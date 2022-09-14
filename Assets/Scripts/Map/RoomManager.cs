using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomManager : MonoBehaviour
{
    enum Object {Enemy,Npc,Reward}
    public RoomData roomData;
    [SerializeField] Transform gatePointer;
    [SerializeField] Transform enemyPointer;
    [SerializeField] Transform npcPointer;
    [SerializeField] Transform rewardPointer;
    [SerializeField] Transform upGate;
    [SerializeField] Transform downGate;
    [SerializeField] Transform leftGate;
    [SerializeField] Transform rightGate;
    Dictionary<Object, List<Transform>> SpawnPoint = new Dictionary<Object, List<Transform>>();
    Dictionary<MapManager.GateDirection,Transform> Gate = new Dictionary<MapManager.GateDirection,Transform>();

    void CreateSpawnPoint()
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
    }

    private List<Transform> CreateChildList(Transform transform)
    {
        List<Transform> childList = new List<Transform>();
        for(int i = 0; i < transform.childCount; i++)
        {
            childList.Add(transform.GetChild(i));
        }
        return childList;
    }

    void PlayerSpawn(Transform player,MapManager.GateDirection direction)
    {
        player.position = Gate[direction].position;
    }

    void EnemySpawn()
    {
        int count = SpawnPoint[Object.Enemy].Count;
        if (count > 0)
        {
            foreach (Transform enemy in SpawnPoint[Object.Enemy])
            {
                AddressObject.RandonInstinate(roomData.map_pack).transform.position = enemy.position;
            }
        }
    }

    void NPCSpawn()
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
    }

    void RewardSpawn()
    {
        List<GameObject> rewards = AddressObject.LimitRandomInstinates(roomData.reward_pack, SpawnPoint[Object.Reward].Count);
    }
}
