using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="PlayerData")]
public class PlayerInfo : ScriptableObject
{
    public string player_name;
    public Equip curEquip;
    public List<Item> inventory;
    public float player_maxHp;
    public float player_curHp;




}
