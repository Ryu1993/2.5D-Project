using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName ="Mapdata/Room")]
public class RoomData : ScriptableObject
{
    public enum State { Battle,Shop,Boss}
    public State curState;
    public AssetLabelReference map_label;
    public AssetLabelReference monster_pack;
    public AssetLabelReference npc_pack;




}
