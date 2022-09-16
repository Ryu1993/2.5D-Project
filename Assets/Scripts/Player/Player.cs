using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Player : Singleton<Player>
{
    public enum State { Up, Down, Horizontal, HoUp, HoDown ,Move,Idle,Hit,Attack,Skill,Dash}
    private State[] directionsIdle = { State.Up, State.Down, State.Horizontal, State.HoUp, State.HoDown,State.Idle };
    public Transform spriteTransform;
    public Animator animator;
    public float moveSpeed;
    [SerializeField]
    private Transform mousePointer;
    [SerializeField]
    private Rigidbody rigi;
    private StateMachine<State, Player> stateMachine;
    public Vector3 moveVec;
    private float moveX;
    private float moveZ;
    private State curDirection;
    public UnityAction<State> ChangeState;
    public UnityAction PlayerKinematic;
    public UnityAction PlayerMove;
    private UnityAction<Vector3> CheckRight;

    public State curAction;
    public UnityAction ActionCheck;

    protected override void Awake()
    {
        base.Awake();
        Setting();
    }

    void Setting()
    {
        stateMachine = new StateMachine<State, Player>(this);
        stateMachine.AddState(State.Idle, new PlayerStates.DirectionState());
        stateMachine.AddState(State.Move, new PlayerStates.MoveState());
        #region UnityActionTemplate
        ChangeState += (state) => { stateMachine.ChangeState(state); };
        CheckRight += (target) => { if (transform.position.x < target.x) spriteTransform.localScale = new Vector3(1, 1, 1); else spriteTransform.localScale = new Vector3(-1, 1, 1); };
        PlayerKinematic += () => { rigi.isKinematic = !rigi.isKinematic; };
        PlayerMove += () => { rigi.velocity = moveVec; };
        #endregion
        #region ActionControllerTemplate

        ActionCheck += MoveInput;
        ActionCheck += AttackInput;
        ActionCheck += SkillInput;
        ActionCheck += DashInput;
        curAction = State.Idle;

        #endregion
        curDirection = State.Up;
        ChangeState(curAction);
    }

    public State DirectionState()
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
    public void MoveInput()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        moveVec = new Vector3(moveX, 0, moveZ).normalized * Time.deltaTime * moveSpeed;
        if (moveVec == Vector3.zero)
        {
            curAction = State.Idle;
        }
        else
        {
            curAction = State.Move;
        }
    }

    public void AttackInput()
    {

    }
    public void DashInput()
    {

    }
    public void SkillInput()
    {

    }


}


