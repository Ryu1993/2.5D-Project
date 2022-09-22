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
    public int player_maxHp;
    public int player_curHp;




}
