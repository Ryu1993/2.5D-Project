using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusIconManager : MonoBehaviour, IReturnable
{
    NewObjectPool.PoolInfo pool;
    [SerializeField]
    UnityEngine.UI.Image statusImage;
    [SerializeField]
    TMPro.TextMeshProUGUI countNum;

    public void SetIcon(Sprite statusIcon,int duration)
    {
        statusImage.sprite = statusIcon;
        countNum.text = duration.ToString();
    }
    public void Count(int duration)
    {
        countNum.text = duration.ToString();
    }
    private void OnDisable()
    {
        statusImage.sprite = null;
        countNum.text = null;
    }

    public void PoolInfoSet(NewObjectPool.PoolInfo pool) => this.pool = pool;
    public void Return()=> NewObjectPool.instance.Return(this.gameObject, pool);

}
