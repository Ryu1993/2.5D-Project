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
        RewardCardWindow window;
        public Item item;
        public bool isPick;
        public RewardManager(Item _item, RewardCardWindow _window)
        {
            window = _window;
            item = _item;
        }

        public void PlayerGetItem()
        {
            if (item.Type == Item.ItemType.Buff)
            {
                BuffItem buff = item as BuffItem;
                StatusManager.instance.StatusEffectCreate[buff.buffType].Invoke(GameManager.instance.scenePlayer, buff.duration);
            }
            if (item.Type == Item.ItemType.Artifact)
            {
                Artifact artifact = item as Artifact;
                ItemManager.instance.PlayerGetArtifact(artifact);
            }
            if (item.Type == Item.ItemType.Equip)
            {
                Equip equip = item as Equip;
                ItemManager.instance.PlayerGetWeapon(equip);
            }
            window.WindowClose();
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
    List<RewardManager> managers = new List<RewardManager>();

    [SerializeField]
    AssetLabelReference testLabel;
    public bool isProgress;
    public bool isComplete;

    public void CloseReward()=> gameObject.SetActive(false);
    public void WindowBlock() => blockScreen.SetActive(!blockScreen.activeSelf);
    public void WindowPop() => animator.SetTrigger("Pop");
    public void WindowClose() => animator.SetTrigger("Close");
    public void CreateReward(AssetLabelReference label)
    {
        ResetReward();
        foreach (RewardCardSet rewardCardSet in cardPositions)
        {
            Item randomItem = AddressObject.RandomInstinateScriptable(label) as Item;
            if (randomItem != null)
            {
                RewardManager rewardManager = new RewardManager(randomItem, this);
                managers.Add(rewardManager);
                rewardCardSet.RewardCardSetting(randomItem, rewardManager.PlayerGetItem);
            }
        }
    }
    public void ResetReward()
    {
        foreach (RewardManager rewardManager in managers) AddressObject.Release(rewardManager.item);
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

    public void OpneReward(AssetLabelReference label)
    {
        isProgress = true;
        transform.gameObject.SetActive(true);
        StartCoroutine(RewardCreatePop(label));
    }
private IEnumerator RewardCreatePop(AssetLabelReference label)
    {
        WindowBlock();
        CreateReward(label);
        WindowPop();
        yield return new WaitUntil(() => isComplete);
        WindowBlock();
        isProgress = false;
    }





}
