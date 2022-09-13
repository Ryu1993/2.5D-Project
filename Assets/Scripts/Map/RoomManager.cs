using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomManager : MonoBehaviour
{
    enum Object {player,enemy,npc,reward}
    [SerializeField]
    RoomData roomData;
    Dictionary<Object, List<Transform>> spawn = new Dictionary<Object, List<Transform>>();
    Object[] objects = {Object.player,Object.enemy,Object.npc,Object.reward};


    private void Awake()
    {
        CreateDic();
    }

    void CreateDic()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            spawn.Add(objects[i],new List<Transform>());
            Transform temp = transform.GetChild(i);
            if(temp.childCount>0)
            {
                for(int j = 0; j< temp.childCount; j++)
                {
                    spawn[objects[i]].Add(temp.GetChild(j));
                }
            }
            else
            {
                spawn[objects[i]].Add(temp);
            }
        }
    }

    void PlayerSpawn(Transform player)
    {
        player.transform.position = spawn[Object.player][0].position;
    }

    void EnemySpawn()
    {
        foreach(Transform enemy in spawn[Object.enemy])
        {
            AddressObject.RandonInstinate(roomData.map_pack).transform.position = enemy.position;
        }
    }

    void NPCSpawn()
    {
        int count = spawn[Object.npc].Count - 1;
        if (count>0)
        {
            List<GameObject> npc = AddressObject.LimitInstinates(roomData.npc_pack, count);
            for(int i = 0; i < npc.Count; i++)
            {
                npc[i].transform.position = spawn[Object.npc][i].position;
            }

        }
    }

    void RewardSpawn()
    {
        List<GameObject> rewards = AddressObject.LimitRandomInstinates(roomData.reward_pack,spawn[Object.reward].Count);

    }



}
