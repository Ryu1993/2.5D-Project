using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerInfo playerInfo;
    public Player scenePlayer;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }



}
