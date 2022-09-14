using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName ="MapData/Room")]
public class RoomData : ScriptableObject
{
    public enum State { Battle,Shop,Boss}
    [SerializeField]
    private State _curState;
    public State curState { get { return _curState; } }
    public AssetLabelReference map_pack;
    public AssetLabelReference monster_pack;
    public AssetLabelReference npc_pack;
    public AssetLabelReference reward_pack;

}
