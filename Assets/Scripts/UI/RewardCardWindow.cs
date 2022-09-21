using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using Unity.VisualScripting;

public class RewardCardWindow : MonoBehaviour
{
    [System.Serializable]
    public class RewardManager
    {
        public Item item;
        public bool isPick;
        public RewardManager(Item _item)=> item = _item;
        public void PlayerGetItem()
        {
            Debug.Log("리워드클릭테스트");
        }
    }
    Player player;
    [SerializeField]
    Animator animator;
    [SerializeField]
    GameObject blockScreen;
    [SerializeField]
    RewardCardSet[] cardPositions;
    [HideInInspector]
    public int count;
    [SerializeField]
    List<RewardManager> managers = new List<RewardManager> ();

    [SerializeField]
    AssetLabelReference testLabel;
    public bool isProgress;
    public bool isComplete;

    public void WindowBlock() => blockScreen.SetActive(!blockScreen.activeSelf);
    public void WindowPop() => animator.SetTrigger("Pop");
    public void WindowClose() => animator.SetTrigger("Close");
    public void CreateReward(AssetLabelReference label)
    {
        ResetReward();
        foreach(RewardCardSet rewardCardSet in cardPositions)
        {
            Item randomItem = AddressObject.RandomInstinateScriptable(label) as Item;
            if (randomItem != null)
            {
                RewardManager rewardManager = new RewardManager(randomItem);
                managers.Add(rewardManager);
                rewardCardSet.RewardCardSetting(randomItem, rewardManager.PlayerGetItem);
            }
        }
    }
    public void ResetReward()
    {
        foreach (RewardManager rewardManager in managers)
        {
            AddressObject.Release(rewardManager.item);
        }
        managers.Clear();
    }
    public void CountReset() => count = 0;
    public void CardRevers()
    {
        Transform temp = cardPositions[count].transform;
        temp.GetChild(0).gameObject.SetActive(false);
        temp.GetChild(1).gameObject.SetActive(true);
        count++;
    }
    public void ScaleMax() => transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    public void ScaleMin() => transform.GetComponent<RectTransform>().localScale = new Vector3(0, 0, 0);
    public void RewardTest()
    {
        transform.gameObject.SetActive(true);
        if(!isProgress)
        {
            StartCoroutine(CoRewardTest());
        }
    }
    private IEnumerator CoRewardTest()
    {
        isProgress = true;
        WindowBlock();
        CreateReward(testLabel);
        WindowPop();
        while(!isComplete)
        {
            yield return null;
        }
        WindowBlock();
        isProgress = false;
        ResetReward();
    }

}
