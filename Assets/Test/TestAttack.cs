using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAttack : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    [SerializeField]
    Material material;
    public float tempFloat;
    public bool tempbool;
    WaitForSeconds wait = new WaitForSeconds(0.1f);
    static readonly int desolve = Shader.PropertyToID("_Desolve");

    private void Start()
    {
        StartCoroutine(AttackDelay());
        StartCoroutine(WeaponCreate());
    }

    IEnumerator AttackDelay()
    {
        float count = 0.5f;
        while(count>0)
        {
            if (Input.GetMouseButton(0))
            {
                count = 0.5f;
                animator.SetBool("Active", true);
                animator.SetTrigger("Action1");
            }
            count -= 0.02f;
            yield return new WaitForFixedUpdate();
        }
        animator.SetBool("Active", false);
    }
    IEnumerator WeaponCreate()
    {
        while(!tempbool)
        {
            material.SetFloat(desolve, tempFloat);
            yield return null;
        }

    }



}
