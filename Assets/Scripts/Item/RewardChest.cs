using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;


public class RewardChest : MonoBehaviour,IReturnable
{
    public UnityEngine.AddressableAssets.AssetLabelReference label;
    private NewObjectPool.PoolInfo home;
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private SpriteRenderer sprite;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Collider chestColli;
    public UnityAction openEvent;
    public UnityAction closeEvent;
    private int curLevel;
    private readonly int open = Animator.StringToHash("Open");

    public void RewardSet(UnityEngine.AddressableAssets.AssetLabelReference _label,int level)
    {
        label = _label;
        curLevel = level;
        if(curLevel!=0) animator.SetLayerWeight(curLevel, 1);
        sprite.sprite = sprites[curLevel];
        animator.enabled = true;
        chestColli.isTrigger = false;
    }
    public void OpenChest()
    {
        animator.SetTrigger(open);
        TriggerOn();
        openEvent?.Invoke();
        UIManager.instance.rewardCardWindow.OpneReward(label,closeEvent);
    }
    public void TriggerOn() => chestColli.isTrigger = true;
    public void OnCollisionEnter(Collision collision) => OpenChest();
    public void PoolInfoSet(NewObjectPool.PoolInfo pool)=>home = pool;
    public void Return()
    {
        label = null;
        sprite.sprite = null;
        animator.enabled = false;
        openEvent = null;
        closeEvent = null;
        if (curLevel != 0) animator.SetLayerWeight(curLevel, 0);
        NewObjectPool.instance.Return(this.gameObject, home);
    }


}
