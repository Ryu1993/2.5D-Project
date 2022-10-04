using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatusManager : Singleton<StatusManager>
{
    public Dictionary<AsyncState.Type,UnityAction<Character,int>> StatusEffectCreate = new Dictionary<AsyncState.Type,UnityAction<Character,int>>();
    public StatusContainerUi playerStatusUI;

    protected override void Awake()
    {
        base.Awake();
        StatusEffectCreate.Add(AsyncState.Type.Burn,(character,duration)=>new AsyncState.Burn(duration,character));
        StatusEffectCreate.Add(AsyncState.Type.Heal,(character,duration)=>new AsyncState.Heal(duration,character));
        StatusEffectCreate.Add(AsyncState.Type.Sturn,(character,duration)=>new AsyncState.Sturn(duration,character));
    }



}
