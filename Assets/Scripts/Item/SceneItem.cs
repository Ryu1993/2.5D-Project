using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneItem : MonoBehaviour
{
    public Item item;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    bool isGet;
    UnityEngine.Events.UnityAction<Player> getItemEvent;

    public void SceneItemSet()
    {
        spriteRenderer.sprite = item.cardIcon;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (isGet) return;
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null) getItemEvent(player);
    }




}
