using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class StatusContainerUi : MonoBehaviour
{

    [System.Serializable]
    private class StatusSpriteMatch
    {
        public AsyncState.Type status;
        public Sprite sprite;
    }

    [SerializeField]
    StatusSpriteMatch[] statusMatchList;
    [SerializeField]
    GameObject statusIcon;
    NewObjectPool.PoolInfo statusIconKey;
    Dictionary<AsyncState.Type,Sprite> statusSprite = new Dictionary<AsyncState.Type,Sprite>();

    private void Awake()
    {
        foreach (StatusSpriteMatch statusSpriteMatch in statusMatchList) statusSprite.Add(statusSpriteMatch.status, statusSpriteMatch.sprite);
    }
    private IEnumerator Start()
    {
        yield return new WaitUntil(() => StatusManager.instance != null);
        StatusManager.instance.playerStatusUI = this;
        yield return new WaitUntil(() => NewObjectPool.instance != null);
        statusIconKey = NewObjectPool.instance.PoolInfoSet(statusIcon, 6, 3);
    } 
    public StatusIconManager StatusAdd(AsyncState.Type type,int duration)
    {
        NewObjectPool.instance.Call(statusIconKey,transform).TryGetComponent<StatusIconManager>(out var manager);
        manager.SetIcon(statusSprite[type],duration);
        return manager;
    }

}
