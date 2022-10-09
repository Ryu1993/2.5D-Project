using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SceneItem : MonoBehaviour,IReturnable
{
    public Item item;
    private Player player;
    private NewObjectPool.PoolInfo home;
    private bool isGet;
    private GameObject particle;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    private UnityAction<Player> getItemEvent;
    public UnityAction closeEvent;
    [SerializeField]
    private TMPro.TextMeshProUGUI textScript;
    [SerializeField]
    private TMPro.TextMeshProUGUI nameScript;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera objectCamera;
    [SerializeField]
    private RectTransform itemInfoUI;
    [SerializeField]
    private Animator animator;
    public Coroutine closeCoroutine;
    private readonly int popup = Animator.StringToHash("PopUp");
    private readonly int close = Animator.StringToHash("Close");


    protected void Awake()
    {
        if(item != null) SceneItemSet(item);
    }


    public void SceneItemSet(Item item)
    {
        closeCoroutine = null;
        this.item = item;
        if (item.Type == Item.ItemType.Buff)
        {
            getItemEvent = getBuff;
            particle = Instantiate(item.itemPrefab, transform);
        }
        else
        {
            if (item.Type == Item.ItemType.Equip) getItemEvent = getEquip;
            if (item.Type == Item.ItemType.Artifact) getItemEvent = getArtifact;
            spriteRenderer.sprite = item.cardIcon;

        }
        nameScript.text = item.Name;
        textScript.text = item.simpleOptionText;
    }

    public void ResetItem()
    {
        item = null;
        player = null;
        isGet = false;
    }

    protected void getEquip(Player player)
    {
        Equip equip = item as Equip;
        ItemManager.instance.PlayerGetWeapon(equip);
        Close();
    }

     protected void getBuff(Player player)
    {
        BuffItem buff = item as BuffItem;
        StatusManager.instance.StatusEffectCreate[buff.buffType].Invoke(player, buff.duration);
        Close();
    }

    protected void getArtifact(Player player)
    {
        Artifact artifact = item as Artifact;
        ItemManager.instance.PlayerGetArtifact(artifact);
        Close();     
    }

    public void Select() => getItemEvent?.Invoke(player);

    public void Cancle() => animator.SetTrigger(close);

    public void Close()
    {
        Cancle();
        closeCoroutine = StartCoroutine(WaitAnimation());
    }
 
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
    protected void OnCollisionEnter(Collision collision)
    {
        player = collision.gameObject.GetComponent<Player>();
        if (player == null) return;
        if (isGet) return;
        isGet = true;
        Time.timeScale = 0f;
        animator.SetTrigger(popup);
    }
    protected void OnCollisionExit(Collision collision)
    {
        if (!isGet) return;
        isGet=false;
    }

    protected virtual IEnumerator WaitAnimation()
    {
        yield return WaitList.isPlay;
        closeEvent?.Invoke();
        closeEvent = null;
        ResetItem();
        Return();
    }

    public void PoolInfoSet(NewObjectPool.PoolInfo pool)=> home = pool;
  
    public void Return()
    {
        if (particle != null)
        {
            DestroyImmediate(particle);
            particle = null;
        }
        NewObjectPool.instance.Return(this.gameObject, home);
    }


}
