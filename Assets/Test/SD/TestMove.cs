using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMove : MonoBehaviour
{
    [SerializeField]
    Transform pointer;
    [SerializeField]
    Animator animator;
    [SerializeField]
    Rigidbody rigi;
    [SerializeField]
    float moveSpeed;
    float moveZ;
    float moveX;
    Vector3 moveVec;


    private void Update()
    {
        Move();
    }

    public void Move()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        if(moveX!=0||moveZ!=0)
        {
            animator.SetBool("IsMove", true);
        }
        else
        {
            animator.SetBool("IsMove", false);
            rigi.velocity = Vector3.zero;
        }
        moveVec = new Vector3(moveX,0,moveZ)*moveSpeed;
        rigi.velocity = moveVec;
        transform.LookAt(new Vector3(pointer.position.x, transform.position.y, pointer.position.z));
    }


}
