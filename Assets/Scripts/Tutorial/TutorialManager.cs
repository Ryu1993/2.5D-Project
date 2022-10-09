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
    AssetLabelReference baseArtifact;
    [SerializeField]
    PlayableDirector tutorialDirector;
    [SerializeField]
    TutorialGate tutorialGate;
    [SerializeField]
    GameObject playerCamera;
    int monsterCount = 3;

    protected override void Awake()
    {
        instance = null;
        base.Awake();
        StartCoroutine(PlayerReset());
    }

    private void Start()
    {
        ItemManager.instance.CreateSceneItem(baseWeapon, new Vector3(-3,0,0));
        StartCoroutine(WeaponGetEvent());
    }

    private IEnumerator PlayerReset()
    {
        yield return new WaitUntil(() => !LoadingUI.instance.gameObject.activeSelf);
        yield return WaitList.isGameManagerSet;
        GameManager.instance.PlayerInfoReset();
        GameManager.instance.scenePlayer = scenePlayer;
        scenePlayer.PlayerSetting();
    }

    public void TutorialMonsterEvent()
    {
        monsterCount--;
        if(monsterCount==0)
        {
            RewardChest chest = ItemManager.instance.CreateRewardChest(baseArtifact, Vector3.zero, 0);
            chest.closeEvent = chest.Return;
            chest.TryGetComponent(out BoxCollider box);
            StartCoroutine(RewardGetEvent(box));
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
