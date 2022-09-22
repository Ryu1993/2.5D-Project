using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private PlayerInfo savePlayerInfo;
    public PlayerInfo playerInfo;
    public Player scenePlayer;
    public bool isSetComplete;
 

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        PlayerInfoSet();
    }

    private void PlayerInfoSet()
    {
        playerInfo.player_name = savePlayerInfo.player_name;
        playerInfo.player_maxHp = savePlayerInfo.player_maxHp;
        playerInfo.player_curHp = savePlayerInfo.player_curHp;
        playerInfo.curEquip = savePlayerInfo.curEquip;
        foreach(Item item in savePlayerInfo.inventory)
        {
            playerInfo.inventory.Add(item);
        }
        isSetComplete = true;
    }
    public void PlayerInfoSave()
    {
        savePlayerInfo.player_maxHp = playerInfo.player_maxHp;
        savePlayerInfo.player_curHp = playerInfo.player_curHp;
        savePlayerInfo.curEquip = playerInfo.curEquip;
        savePlayerInfo.inventory.Clear();
        foreach(Item item in playerInfo.inventory)
        {
            savePlayerInfo.inventory.Add(item);
        }
    }



}
