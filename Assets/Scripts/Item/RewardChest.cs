using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class RewardChest : MonoBehaviour
{
    public UnityEngine.AddressableAssets.AssetLabelReference label;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Collider chestColli;
    public UnityAction openEvent;
    private readonly int open = Animator.StringToHash("Open");

    public void RewardSet(UnityEngine.AddressableAssets.AssetLabelReference _label) => label = _label;
    public void OpenChest()
    {
        animator.SetTrigger(open);
        TriggerOn();
        openEvent?.Invoke();
        UIManager.instance.rewardCardWindow.OpneReward(label);
        if (MapManager.instance != null)
        {
            MapManager.instance.CurMapClear();
        }
    }
    public void TriggerOn() => chestColli.isTrigger = true;

    public void OnCollisionEnter(Collision collision) => OpenChest();

}
