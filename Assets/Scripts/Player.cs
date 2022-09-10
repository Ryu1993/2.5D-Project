using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
 
    public Transform spriteTransform;
    public Animator animator;
    [SerializeField]
    Transform mousePointer;
    [SerializeField]
    Rigidbody rigi;
    public enum State {Up,Down,Horizontal,HoUp,HoDown }
    StateMachine<State, Player> stateMachine;
    float dot;
    Vector3 moveVec;
    float moveX;
    float moveZ;
    [SerializeField]
    float moveSpeed;

    private void Awake()
    {
        SettingStateMachine();
    }

    void SettingStateMachine()
    {
        stateMachine = new StateMachine<State, Player>(this);
        stateMachine.AddState(State.Up, new PlayerStates.UpState());
        stateMachine.AddState(State.Down, new PlayerStates.DownState());
        stateMachine.AddState(State.Horizontal, new PlayerStates.HorizontalState());
        stateMachine.AddState(State.HoUp, new PlayerStates.HoUpState());
        stateMachine.AddState(State.HoDown, new PlayerStates.HoDownSate());
        ChangeState(State.Down);
    }

    public void ChangeState(State nextState)
    {
        stateMachine.ChangeState(nextState);
    }

    public void CheckLeftOrRinght(Vector3 target)
    {
        if(transform.position.x<target.x)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public State RotateState()
    {
        CheckLeftOrRinght(mousePointer.position);
        dot = Vector3.Dot(transform.forward, (mousePointer.position-transform.position).normalized);
        if (dot > 0.866) return State.Up;
        else if (dot > 0.5) return State.HoUp;
        else if (dot > -0.5) return State.Horizontal;
        else if (dot > -0.866) return State.HoDown;
        else return State.Down;
    }

    public void Move()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        moveVec = new Vector3(moveX, 0, moveZ).normalized * Time.deltaTime * moveSpeed;
        rigi.velocity = moveVec;
    }

}


