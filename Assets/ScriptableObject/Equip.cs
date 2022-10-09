using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Equip")]
public class Equip : Item
{
    [Header("¹«±â½ºÅÝ")]
    public LayerMask layerMask;
    public float damage;
    public float attackCooltime;
    public float attackRange;
    public int maxCombo;
    public bool isSuperArmor;

 
}
