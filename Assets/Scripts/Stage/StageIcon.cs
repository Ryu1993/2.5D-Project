using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageIcon : MonoBehaviour
{
    private int onSelect = Animator.StringToHash("OnSelect");
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private BoxCollider boxCollider;
    public StageManager.MapConnetInfo mapConnetInfo;
    public bool isAcitve;
    private float count = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    public void CurMapConnetInfo(StageManager.MapConnetInfo info)
    {
        mapConnetInfo = info;
        if (info.curMap!=null) spriteRenderer.sprite = info.curMap.iconSprite;

    }

    public void Enable() => boxCollider.enabled = true;
    public void Disable() => boxCollider.isTrigger = true;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        Animator animator = other.GetComponent<Animator>();
        animator.SetBool("Move", false);
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player") return;
        count += Time.fixedDeltaTime;
        if(count>=0.5f)
        {
            boxCollider.enabled = false;
            GameManager.instance.playerInfo.curMapInfo = mapConnetInfo.curMap;
            AsyncOperation operation = SceneManager.LoadSceneAsync("GameScene");
            operation.allowSceneActivation = false;
            LoadingUI.instance.CallLoading(operation);
        }
 
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "GameController") animator.SetBool(onSelect, true);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "GameController")
        {
            if(Input.GetMouseButton(0))
            {
                animator.SetBool(onSelect, false);
                StageManager.instance.IconSelected(transform.position);
            }     
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "GameController") animator.SetBool(onSelect, false);
    }



}
