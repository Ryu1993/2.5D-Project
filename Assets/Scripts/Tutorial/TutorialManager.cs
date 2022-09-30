using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class TutorialManager : Singleton<TutorialManager>
{
    [SerializeField]
    Player scenePlayer;
    [SerializeField]
    GameObject sceneItem;
    [SerializeField]
    Equip baseWeapon;
    [SerializeField]
    Artifact baseArtifact;
    [SerializeField]
    GameObject tutorialReward;
    int monsterCount = 3;
    NewObjectPool.PoolInfo pool;

    protected override void Awake()
    {
        instance = null;
        base.Awake();
        StartCoroutine(PlayerReset());
    }

    private void Start()
    {
        pool = NewObjectPool.instance.PoolInfoSet(sceneItem, 1, 1);
        SceneItem go = NewObjectPool.instance.Call(pool, Vector3.zero).GetComponent<SceneItem>();
        go.SceneItemSet(baseWeapon);
    }

    private IEnumerator PlayerReset()
    {
        yield return WaitList.isGameManagerSet;
        GameManager.instance.playerInfo.curEquip = null;
        GameManager.instance.playerInfo.inventory.Clear();
        GameManager.instance.playerInfo.player_maxHp = 6;
        GameManager.instance.playerInfo.player_curHp = 6;
        GameManager.instance.playerInfo.curStage = 0;
        GameManager.instance.scenePlayer = scenePlayer;
        scenePlayer.PlayerSetting();
    }

    public void TutorialMonsterEvent()
    {
        monsterCount--;
        if(monsterCount==0)
        {
            tutorialReward.SetActive(true);
        }
    }


}
