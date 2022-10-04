using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;

public class BossMonster : Monster
{
    public UnityAction<float> bossHpEvent;
    public override float curHp
    { 
        get => base.curHp; 
        set
        {
            base.curHp = value;
            bossHpEvent?.Invoke(value);
        }
    }

}
