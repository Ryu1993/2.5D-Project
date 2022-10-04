using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField]
    Player scenePlayer;
    [SerializeField]
    StatusContainerUi statusContainerUi;
    [SerializeField]
    MapManager mapManager;


    private IEnumerator Start()
    {
        yield return WaitList.isGameManagerSet;
        GameManager.instance.scenePlayer = scenePlayer;
        StatusManager.instance.playerStatusUI = statusContainerUi;
        scenePlayer.PlayerSetting();
        mapManager.GameEneter(GameManager.instance.playerInfo.curStageInfo.curMap);
    }
}
