using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItem : MonoBehaviour,IReturnable
{
    public Item item;
    Player player;
    NewObjectPool.PoolInfo home;
    bool isGet;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    UnityEngine.Events.UnityAction<Player> getItemEvent;
    [SerializeField]
    TMPro.TextMeshProUGUI textScript;
    [SerializeField]
    TMPro.TextMeshProUGUI nameScript;
    [SerializeField]
    Cinemachine.CinemachineVirtualCamera objectCamera;
    [SerializeField]
    RectTransform itemInfoUI;
    [SerializeField]
    Animator animator;



    public void SceneItemSet(Item item)
    {
        this.item = item;
        spriteRenderer.sprite = item.cardIcon;
        nameScript.text = item.Name;
        textScript.text = item.simpleOptionText;
        if (item.Type == Item.ItemType.Equip) getItemEvent = getEquip;
        if (item.Type == Item.ItemType.Buff) { getItemEvent = getBuff; }
        if(item.Type == Item.ItemType.Artifact) getItemEvent = getArtifact;
    }

    public void ResetItem()
    {
        item = null;
        player = null;
        isGet = false;
    }

    private void getEquip(Player player)
    {
        Equip equip = item as Equip;
        ItemManager.instance.PlayerGetWeapon(equip);
        Cancle();
        StartCoroutine(WaitAnimation());
    }

    private void getBuff(Player player)
    {
        BuffItem buff = item as BuffItem;
        StatusManager.instance.StatusEffectCreate[buff.buffType].Invoke(player, buff.duration);
        Cancle();
        StartCoroutine(WaitAnimation());
    }

    private void getArtifact(Player player)
    {
        Artifact artifact = item as Artifact;
        ItemManager.instance.PlayerGetArtifact(artifact);
        Cancle();
        StartCoroutine(WaitAnimation());
    }

    public void Select() => getItemEvent?.Invoke(player);

    public void Cancle() => animator.SetTrigger("Close");
 
    public void ItemInfoUIPop()
    { 
        itemInfoUI.localScale = new Vector3(1, 1, 1);
        objectCamera.gameObject.SetActive(true);
    }
    public void ItemInfoUIClose()
    {
        itemInfoUI.localScale = Vector3.zero;
        objectCamera.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    private void OnCollisionEnter(Collision collision)
    {
        player = collision.gameObject.GetComponent<Player>();
        if (player == null) return;
        if (isGet) return;
        isGet = true;
        Time.timeScale = 0f;
        animator.SetTrigger("PopUp");
    }

    private void OnCollisionExit(Collision collision)
    {
        if (!isGet) return;
        isGet=false;
    }

    private IEnumerator WaitAnimation()
    {
        yield return WaitList.isPlay;
        ResetItem();
        Return();
    }

    public void PoolInfoSet(NewObjectPool.PoolInfo pool)=> home = pool;
  
    public void Return() => NewObjectPool.instance.Return(this.gameObject, home);

}
