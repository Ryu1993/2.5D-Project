using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Item/Buff")]
public class BuffItem : Item
{

    public AsyncState.Type buffType;
    public int duration;
}
