using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    NewObjectPool.PoolInfo sceneItemKey;
    [SerializeField]
    Player player;

    protected override void Awake()
    {
        base.Awake();
    }

    public void PlayerGetArtifact(Artifact artifact)
    {
        if (player == null) player = GameManager.instance.scenePlayer;



    }

    public void PlayerGetWeapon(Equip equip)
    {
        if (player == null) player = GameManager.instance.scenePlayer;



    }

    




}
