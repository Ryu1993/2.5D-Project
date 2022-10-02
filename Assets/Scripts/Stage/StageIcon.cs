using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageIcon : MonoBehaviour
{
    private int onSelect = Animator.StringToHash("OnSelect");
    [SerializeField]
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
    }
    public void CurMapConnetInfo(StageManager.MapConnetInfo info)
    {
        mapConnetInfo = info;
        spriteRenderer.sprite = info.curMap.iconSprite;
    }

    public void Enable() => boxCollider.enabled = true;
    public void Disable() => boxCollider.enabled = false;


    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Animator>().SetBool("Move", false);
    }
    private void OnTriggerStay(Collider other)
    {
        count += Time.fixedDeltaTime;
        if(count>=0.5f)
        {

        }
 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10) animator.SetBool(onSelect, true);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            if(Input.GetMouseButton(0))
            {
                StageManager.instance.IconSelected(transform.position);
                boxCollider.isTrigger = true;
            }     
        }
    }


}
