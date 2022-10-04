using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpContainerUI : MonoBehaviour
{
    [SerializeField]
    Image[] heartList;
    [SerializeField]
    float curHeart = 0;
    [SerializeField]
    float maxHeart = 0;

    IEnumerator Start()
    {
        yield return WaitList.isGameManagerSet;
        yield return new WaitUntil(() => GameManager.instance.scenePlayer!=null);
        yield return new WaitUntil(() => GameManager.instance.scenePlayer.isReady);
        Player player = GameManager.instance.scenePlayer;
        while(true)
        {
            if (player.isReady) break;
            yield return null;
        }
        foreach (Image heart in heartList) heart.fillAmount = 0f;
        AddMaxHeart(player.maxHp);
        AddHeart(player.curHp);
        player.hpUp += HeartChange;
        player.maxHpUp += AddMaxHeart;
    }
    public void HeartChange(float num)
    {
        if(num>=curHeart)AddHeart(num-curHeart);
        if (num < curHeart) DamageHeart(curHeart - num);
    }
    public void AddMaxHeart(float num) => maxHeart+=num;
    public void DamageHeart(float num)
    {
        for(int i = 0; i < num; i++)
        {
            curHeart--;
            heartList[(int)curHeart / 2].fillAmount -= 0.5f;
        }
    }
    public void AddHeart(float num)
    {
        for (int i = 0; i < num; i++)
        {
            heartList[(int)curHeart / 2].fillAmount += 0.5f;
            curHeart++;
        }
    }

}
