using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterBehaviourManager : Singleton<MonsterBehaviourManager>
{
    private Player secenePlayer;
    public Vector3 playerPosition;
    public UnityAction monsterBehaviour;
    public Coroutine coFixeUpdate;

    protected override void Awake()
    {
        base.Awake();    
    }


    private void Start()
    {
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

 
}
