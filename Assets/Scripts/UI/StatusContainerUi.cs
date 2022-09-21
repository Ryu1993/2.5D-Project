using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusContainerUi : Singleton<StatusContainerUi>
{
    [SerializeField]
    Sprite[] statusSprites;
    [SerializeField]
    GameObject statusIcon;
    NewObjectPool.PoolInfo statusIconKey;
    Dictionary<AsyncState.Type,Sprite> statusSprite = new Dictionary<AsyncState.Type,Sprite>();

    protected override void Awake()
    {
        base.Awake();
        DicInfoSet();
    }
    private void Start()
    {
        statusIconKey = NewObjectPool.instance.PoolInfoSet(statusIcon,6,3);
    }

    public void DicInfoSet() // sprite �� ���⼭ ��Ī
    {
        statusSprite.Add(AsyncState.Type.Burn, statusSprites[0]);
    }



    public StatusIconManager StatusAdd(AsyncState.Type type,int duration)
    {
        StatusIconManager manager = NewObjectPool.instance.Call(statusIconKey,transform).GetComponent<StatusIconManager>();
        manager.SetIcon(statusSprite[type],duration);
        return manager;
    }

}
