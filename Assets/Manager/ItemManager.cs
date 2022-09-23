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
    NewObjectPool.PoolInfo sceneItemkey;



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
    }

    public void PlayerGetArtifact(Artifact artifact)
    {
        if (ArtifactEffet.ContainsKey(artifact.artifactType)) ArtifactEffet[artifact.artifactType]?.Invoke(artifact.strength);
        else Debug.Log("아직 정해지지않은 아티팩트 효과");
        UIManager.instance.artifactUI.ArtifactDraw(artifact);
    }

    public void PlayerGetWeapon(Equip equip)
    {
        UIManager.instance.weaponUI.ChangeWeapon(equip);
        player.weaponContainer.WeaponSet(equip.weaponPrefab);
    }

    public void CreateSceneItem(Item item, Vector3 position)
    {
        if (sceneItemkey == null) sceneItemkey = NewObjectPool.instance.PoolInfoSet(sceneItem,3,1);
        SceneItem createdItem =  NewObjectPool.instance.Call(sceneItemkey, position).GetComponent<SceneItem>();
        createdItem.SceneItemSet(item);
    }

    
    public void ArtifactEffectAdd()
    {

    }





}
