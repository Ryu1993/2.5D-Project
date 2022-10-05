using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterBehaviourManager : Singleton<MonsterBehaviourManager>
{
    [SerializeField]
    private Player secenePlayer;
    public Vector3 playerPosition;
    public UnityAction monsterBehaviour;
    public Coroutine coFixeUpdate;
    [SerializeField]
    private GameObject monsterBullet;
    [SerializeField]
    bool isTest;
    private NewObjectPool.PoolInfo bulletInfo;
    

    protected override void Awake()
    {
        instance = null;
        base.Awake();
    }


    private IEnumerator Start()
    {
        if(isTest)
        {
            StartCoroutine(CoFixedUpdate());
            yield break;
        }
        yield return WaitList.isGameManagerSet;
        yield return WaitList.isPlayerSet;
        yield return WaitList.isPlayerReady;
        secenePlayer = GameManager.instance.scenePlayer;
        StartCoroutine(CoFixedUpdate());

    } 
 
    private IEnumerator CoFixedUpdate()
    {
        while (true)
        {
            playerPosition = secenePlayer.transform.position;
            monsterBehaviour?.Invoke();
            yield return WaitList.fixedUpdate;
        }

    }

    public NewObjectPool.PoolInfo RequestBullet()
    {
        if (bulletInfo == null) bulletInfo = NewObjectPool.instance.PoolInfoSet(monsterBullet, 30, 15);
        return bulletInfo;    
    }


 
}
