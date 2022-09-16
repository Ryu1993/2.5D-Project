using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Player : Singleton<Player>
{
    public enum State { Up, Down, Horizontal, HoUp, HoDown }

    public Transform spriteTransform;
    public Animator animator;
    public float moveSpeed;
    [SerializeField]
    private Transform mousePointer;
    [SerializeField]
    private Rigidbody rigi;
    private StateMachine<State, Player> stateMachine;
    private Vector3 moveVec;
    private float moveX;
    private float moveZ;
    private State curDirection;
    private State curState;
    public UnityAction<State> ChangeState;
    public UnityAction PlayerKinematic;
    private UnityAction<Vector3> CheckRight;
    


    protected override void Awake()
    {
        base.Awake();
        Setting();
    }

    void Setting()
    {
        stateMachine = new StateMachine<State, Player>(this);
        stateMachine.AddState(State.Up, new PlayerStates.UpState());
        stateMachine.AddState(State.Down, new PlayerStates.DownState());
        stateMachine.AddState(State.Horizontal, new PlayerStates.HorizontalState());
        stateMachine.AddState(State.HoUp, new PlayerStates.HoUpState());
        stateMachine.AddState(State.HoDown, new PlayerStates.HoDownSate());
        ChangeState += (state) => { stateMachine.ChangeState(state); };
        CheckRight += (target) => { if (transform.position.x < target.x) spriteTransform.localScale = new Vector3(1, 1, 1); else spriteTransform.localScale = new Vector3(-1, 1, 1); };
        PlayerKinematic += () => { rigi.isKinematic = !rigi.isKinematic; };
        ChangeState(State.Up);
    }


    public State RotateState()
    {
        CheckRight(mousePointer.position);
        float dot = Vector3.Dot(transform.forward, (mousePointer.position-transform.position).normalized);
        if (dot > 0.866) curDirection = State.Up;
        else if (dot > 0.5) curDirection = State.HoUp;
        else if (dot > -0.5) curDirection = State.Horizontal;
        else if (dot > -0.866) curDirection = State.HoDown;
        else curDirection = State.Down;
        return curDirection;
    }




    public void Move()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        moveVec = new Vector3(moveX, 0, moveZ).normalized * Time.deltaTime * moveSpeed;
        rigi.velocity = moveVec;
    }



}


