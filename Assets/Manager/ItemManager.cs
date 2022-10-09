using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;

public class ItemManager : Singleton<ItemManager>
{
    Dictionary<Artifact.ArtifactType,UnityAction<float>> ArtifactEffet = new Dictionary<Artifact.ArtifactType,UnityAction<float>>();
    [SerializeField]
    GameObject sceneItem;
    [SerializeField]
    GameObject rewardChest;
    NewObjectPool.PoolInfo sceneItemKey;
    NewObjectPool.PoolInfo rewardChestKey;
    Player _player;
    Player player
    {
        get
        {
            if (_player == null) _player = GameManager.instance.scenePlayer;    
            return _player; 
        } 
    }
    protected override void Awake()
    {
        ArtifactEffectAdd();
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void PlayerGetArtifact(Artifact artifact)
    {
        GameManager.instance.playerInfo.inventory.Add(artifact);
        ArtifactActivation(artifact);
    }
    public void ArtifactActivation(Artifact artifact)
    {
        if (ArtifactEffet.ContainsKey(artifact.artifactType)) ArtifactEffet[artifact.artifactType]?.Invoke(artifact.strength);
        UIManager.instance.artifactUI.ArtifactDraw(artifact);
    }

    public void PlayerGetWeapon(Equip equip)
    {
        UIManager.instance.weaponUI.ChangeWeapon(equip);
        player.weaponContainer.WeaponSet(equip);
        GameManager.instance.playerInfo.curEquip = equip;
    }

    public SceneItem CreateSceneItem(Item item, Vector3 position)
    {
        sceneItemKey = NewObjectPool.instance.PoolInfoSet(sceneItem,3,1);
        NewObjectPool.instance.Call(sceneItemKey, position).TryGetComponent(out SceneItem createdItem);
        createdItem.SceneItemSet(item);
        return createdItem;
    }

    public RewardChest CreateRewardChest(AssetLabelReference label,Vector3 position,int level)
    {
        rewardChestKey = NewObjectPool.instance.PoolInfoSet(rewardChest, 3, 1);
        NewObjectPool.instance.Call(rewardChestKey, position).TryGetComponent(out RewardChest createdChest);
        createdChest.RewardSet(label,level);
        return createdChest;
    }




    public void ArtifactEffectAdd()
    {
        ArtifactEffet.Add(Artifact.ArtifactType.MaxHp, MaxHpUp);
        ArtifactEffet.Add(Artifact.ArtifactType.MoveSpeed,MoveSpeedUp);
        ArtifactEffet.Add(Artifact.ArtifactType.DashLength,DashLengthUp);
        ArtifactEffet.Add(Artifact.ArtifactType.DashCount, DashCountUp);
    }

    private void MaxHpUp(float strength)
    {
        player.maxHp += (float)strength;
        player.curHp += (float)strength;
    }
    private void MoveSpeedUp(float strength)=> player.moveSpeed += strength;
    private void DashLengthUp(float strength)=> player.dashInvincibleTime += strength;
    private void DashCountUp(float strength)
    {
        player.maxDashable += (int)strength;
        player.curDashCount += (int)strength;
    }



}
