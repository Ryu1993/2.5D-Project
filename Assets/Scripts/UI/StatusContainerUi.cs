using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class StatusContainerUi : MonoBehaviour
{
    [SerializeField]
    Sprite[] statusSprites;
    [SerializeField]
    GameObject statusIcon;
    NewObjectPool.PoolInfo statusIconKey;
    Dictionary<AsyncState.Type,Sprite> statusSprite = new Dictionary<AsyncState.Type,Sprite>();

    private void Awake()
    {
        DicInfoSet();
    }
    private void Start()
    {
        statusIconKey = NewObjectPool.instance.PoolInfoSet(statusIcon,6,3);
    }

    public void DicInfoSet() // sprite ¶û ¿©±â¼­ ¸ÅÄª
    {
        statusSprite.Add(AsyncState.Type.Burn, statusSprites[0]);
        statusSprite.Add(AsyncState.Type.Heal, statusSprites[0]);
    }
    public StatusIconManager StatusAdd(AsyncState.Type type,int duration)
    {
        StatusIconManager manager = NewObjectPool.instance.Call(statusIconKey,transform).GetComponent<StatusIconManager>();
        manager.SetIcon(statusSprite[type],duration);
        return manager;
    }

}
