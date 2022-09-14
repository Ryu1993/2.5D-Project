using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName ="MapData/Room")]
public class RoomData : ScriptableObject
{
    public enum RoomType { Event,Battle,Boss,Reward}
    public RoomType curRoomType;
    public Sprite icon;
    public AssetLabelReference map_pack;
    public AssetLabelReference monster_pack;
    public AssetLabelReference npc_pack;
    public AssetLabelReference reward_pack;

}
