using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StatusManager : Singleton<StatusManager>
{
    public Dictionary<AsyncState.Type,UnityAction<Character,int>> StatusEffectCreate = new Dictionary<AsyncState.Type,UnityAction<Character,int>>();
    public StatusContainerUi _playerStatusUI;
    public StatusContainerUi playerStatusUI
    {
        get
        {
            if (_playerStatusUI == null) _playerStatusUI = UIManager.instance.statusUI;
            return _playerStatusUI; 
        }
        set { _playerStatusUI = value; }
    }

    protected override void Awake()
    {
        base.Awake();
        StatusEffectCreate.Add(AsyncState.Type.Burn,(character,duration)=>new AsyncState.Burn(duration,character));
        StatusEffectCreate.Add(AsyncState.Type.Heal,(character,duration)=>new AsyncState.Heal(duration,character));
        StatusEffectCreate.Add(AsyncState.Type.Sturn,(character,duration)=>new AsyncState.Sturn(duration,character));
        StatusEffectCreate.Add(AsyncState.Type.MoveUP,(character,duration)=>new AsyncState.MoveUP(duration,character));
    }



}
