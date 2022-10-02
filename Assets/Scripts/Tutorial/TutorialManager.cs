using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Playables;
using UnityEngine.Timeline;
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
    [SerializeField]
    PlayableDirector tutorialDirector;
    [SerializeField]
    TutorialGate tutorialGate;
    [SerializeField]
    GameObject playerCamera;
    GameObject basicWeapon;

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
        basicWeapon = NewObjectPool.instance.Call(pool, new Vector3(-3, 0, 0)).gameObject;
        SceneItem go = basicWeapon.GetComponent<SceneItem>();
        go.SceneItemSet(baseWeapon);
        StartCoroutine(WeaponGetEvent());
    }

    private IEnumerator PlayerReset()
    {
        yield return new WaitUntil(() => !LoadingUI.instance.gameObject.activeSelf);
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
            StartCoroutine(RewardGetEvent(tutorialReward.GetComponent<BoxCollider>()));
        }
    }

    public void TimeSwitch()
    {
        if (Time.timeScale == 1) Time.timeScale = 0;
        if (Time.timeScale == 0) Time.timeScale = 1;
    }
    private IEnumerator WeaponGetEvent()
    {
        yield return new WaitUntil(() => scenePlayer.weaponContainer.curWeapon != null);
        yield return WaitList.isPlay;
        playerCamera.SetActive(false);
        tutorialDirector.Play();  
    }
    private IEnumerator RewardGetEvent(BoxCollider collider)
    {
        yield return new WaitUntil(() => collider.isTrigger);
        tutorialGate.GateOpne();
        
    }


}
