using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpContainerUI : MonoBehaviour
{
    [SerializeField]
    Image[] heartList;
    int curHeart = 0;
    int _maxHeart = 0;
    public int maxHeart
    {
        get { return _maxHeart; }
        set 
        {
            if(value>12) value = 12;
            _maxHeart = value;
        }
    }
    int curIndex = 0;
    bool isProgress;
    [SerializeField]
    GameManager gameManager;
    private void OnEnable()
    {
        if (isProgress) return;
        StartCoroutine(CoOnEnabel());
    }
    IEnumerator CoOnEnabel()
    {
        isProgress = true;
        yield return WaitList.isGameManagerSet;
        yield return new WaitUntil(() => GameManager.instance.scenePlayer.isReady);
        Player player = GameManager.instance.scenePlayer;
        while(true)
        {
            if (player.isReady) break;
            yield return null;
        }
        AddMaxHeart((int)player.maxHp);
        AddHeart((int)player.curHp);
        player.hpUp += HeartChange;
        player.maxHpUp += AddMaxHeart;
        isProgress = false;
    }
    public void HeartChange(int num)
    {
        if (num >= 0) AddHeart(num);
        else DamageHeart(num);
    }
    public void AddMaxHeart(int num) => maxHeart+=num;
    public void DamageHeart(int num)
    {
        for(int i = 0; i < num; i++)
        {
            heartList[curIndex-1].fillAmount -= 0.5f;
            curHeart--;
            IndexCount(ref curIndex, ref curHeart);
        }
    }
    public void AddHeart(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (curHeart <= 1)
            {
                heartList[0].fillAmount += 0.5f;
            }
            else if (curHeart % 2 == 0)
            {
                heartList[curIndex].fillAmount += 0.5f;
            }
            else
            {
                heartList[curIndex - 1].fillAmount += 0.5f;
            }
            curHeart++;
            IndexCount(ref curIndex, ref curHeart);
        }
    }

    private void IndexCount(ref int curindex,ref int curheart)
    {
        curindex = curheart / 2;
        if (curheart % 2 != 0) curindex = curheart / 2 + 1;
    }
}
