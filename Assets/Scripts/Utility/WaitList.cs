using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WaitList 
{
    public static WaitForFixedUpdate fixedUpdate = new WaitForFixedUpdate();
    public static WaitForEndOfFrame endOfFrame = new WaitForEndOfFrame();
    public static WaitForSeconds oneSecond = new WaitForSeconds(1f);
    public static WaitUntil isPause = new WaitUntil(() => Time.timeScale == 0f);
    public static WaitUntil isPlay = new WaitUntil(() => Time.timeScale > 0f);
    public static WaitUntil isGameManagerSet = new WaitUntil(() => GameManager.instance!=null);
    public static WaitUntil isSingletonSet = new WaitUntil(() => GameManager.instance.isSetComplete);

}
