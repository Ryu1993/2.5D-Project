using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaitList 
{
    public static WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();
    public static WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    public static WaitForSeconds oneSecond = new WaitForSeconds(1f);
    public static WaitForSeconds deciSecond = new WaitForSeconds(0.1f);
    public static WaitForSeconds halfSecond = new WaitForSeconds(0.5f);
    public static WaitForSecondsRealtime realOneSecond = new WaitForSecondsRealtime(1f);
    public static WaitUntil isPause = new WaitUntil(() => Time.timeScale == 0f);
    public static WaitUntil isPlay = new WaitUntil(() => Time.timeScale > 0f);
    public static WaitUntil isGameManagerSet = new WaitUntil(() => GameManager.instance!=null);
    public static WaitUntil isSingletonSet = new WaitUntil(() => GameManager.instance.isSetComplete);
    public static WaitUntil isMonsterManagerSet = new WaitUntil(() => MonsterBehaviourManager.instance != null);
    public static WaitUntil isPlayerSet = new WaitUntil(() => GameManager.instance.scenePlayer != null);
    public static WaitUntil isPlayerReady = new WaitUntil(() => GameManager.instance.scenePlayer.isReady);
    public static WaitUntil isObjectPoolSet = new WaitUntil(() => NewObjectPool.instance != null);
}
