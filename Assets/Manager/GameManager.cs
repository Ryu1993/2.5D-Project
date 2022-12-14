using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    public PlayerInfo savePlayerInfo;
    public PlayerInfo playerInfo;
    public Player scenePlayer;
    public bool isSetComplete;
 

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        PlayerInfoSet();    
    }

    public void OnEnable()=> StartCoroutine(SingletonCheck());
    private void PlayerInfoSet()
    {
        playerInfo.player_name = savePlayerInfo.player_name;
        playerInfo.player_maxHp = savePlayerInfo.player_maxHp;
        playerInfo.player_curHp = savePlayerInfo.player_curHp;
        playerInfo.curEquip = savePlayerInfo.curEquip;
        playerInfo.curStage = savePlayerInfo.curStage;
        playerInfo.inventory.Clear();
        foreach(Item item in savePlayerInfo.inventory) playerInfo.inventory.Add(item);
    }
    public void PlayerInfoSave()
    {
        savePlayerInfo.player_maxHp = playerInfo.player_maxHp;
        savePlayerInfo.player_curHp = playerInfo.player_curHp;
        savePlayerInfo.curEquip = playerInfo.curEquip;
        savePlayerInfo.curStage = playerInfo.curStage;
        savePlayerInfo.inventory.Clear();
        foreach(Item item in playerInfo.inventory) savePlayerInfo.inventory.Add(item);
    }

    public void PlayerInfoReset()
    {
       playerInfo.curEquip = null;
       playerInfo.inventory.Clear();
       playerInfo.player_maxHp = 6;
       playerInfo.player_curHp = 6;
       playerInfo.curStage = 0;
       playerInfo.progressStageInfo = null;
       playerInfo.curStageInfoList = null;
    }

    IEnumerator SingletonCheck()
    {
        isSetComplete = false;
        yield return new WaitUntil(() => UIManager.instance != null);
        yield return new WaitUntil(() => ItemManager.instance != null);
        yield return new WaitUntil(() => StatusManager.instance != null);
        isSetComplete = true;
    }
    public void GameOver()
    {

    }


}
