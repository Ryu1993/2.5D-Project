using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class RewardChest : MonoBehaviour
{
    public AssetLabelReference label;
    [SerializeField]
    Animator animator;
    [SerializeField]
    Collider chestColli;
    static readonly int open = Animator.StringToHash("Open");

    public void OpenChest()
    {
        animator.SetTrigger(open);
        chestColli.isTrigger = true;
        UIManager.instance.rewardCardWindow.OpneReward(label);

    }

    public void OnCollisionEnter(Collision collision) => OpenChest();

}
