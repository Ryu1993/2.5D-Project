using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItem : MonoBehaviour
{
    public Item item;
    Player player;
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
        spriteRenderer.sprite = item.cardIcon;
        nameScript.text = item.name;
        textScript.text = item.simpleOptionText;
        if (item.Type == Item.ItemType.Equip) getItemEvent = getEquip;
        if(item.Type == Item.ItemType.Buff) getItemEvent = getBuff;
        if(item.Type == Item.ItemType.Artifact) getItemEvent = getArtifact;
    }

    private void getEquip(Player player)
    {

    }

    private void getBuff(Player player)
    {

    }

    private void getArtifact(Player player)
    {

    }

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


}