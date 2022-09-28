using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName ="PlayerData")]
public class PlayerInfo : ScriptableObject
{
    public string player_name;
    public Equip curEquip;
    public List<Item> inventory;
    [Range(0f, 12f)]
    public int player_maxHp;
    [Range(0f, 12f)]
    public int player_curHp;
    public int curStage;



}
