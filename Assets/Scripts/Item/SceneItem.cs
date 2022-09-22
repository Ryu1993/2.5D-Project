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

    private void OnCollisionEnter(Collision collision)
    {
        player = collision.gameObject.GetComponent<Player>();
        if (player == null) return;
        if (isGet) return;
        Time.timeScale = 0f;
        objectCamera.gameObject.SetActive(true);

    
    }




}
