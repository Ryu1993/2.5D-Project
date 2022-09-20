using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;

public class RewardCardWindow : MonoBehaviour
{
    public class RewardManager
    {
        Item item;
        public RewardManager(Item _item)=> item = _item;

        public void PlayerGetItem()
        {

        }
    }

    Player player;
    [SerializeField]
    Animator animator;
    [SerializeField]
    GameObject blockScreen;
    [SerializeField]
    Transform[] cardPositions;
    [SerializeField]
    RewardCardSet rewardCardFrame;
    [HideInInspector]
    public int count;
    public void WindowBlock() => blockScreen.SetActive(!blockScreen.activeSelf);
    public void WindowPop() => animator.SetTrigger("Pop");

    public void CreateReward(AssetLabelReference label)
    {
        foreach(Transform transform in cardPositions)
        {
            
            RewardCardSet cardSet = Instantiate(rewardCardFrame.gameObject,transform).GetComponent<RewardCardSet>();

        }

    }

    public void CountReset() => count = 0;
    public void CardRevers()
    {
        Transform temp = cardPositions[count].GetChild(0);
        temp.GetChild(0).gameObject.SetActive(false);
        temp.GetChild(1).gameObject.SetActive(true);
        count++;
    }


}
