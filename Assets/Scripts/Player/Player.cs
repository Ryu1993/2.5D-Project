using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Player : Character
{
    private State[] directionsIdle = { State.Up, State.Down, State.Horizontal, State.HoUp, State.HoDown,State.Idle };
    private State curDirection;
    private StateMachine<State, Player> stateMachine;
    [SerializeField]
    private Transform spriteTransform;
    private float moveX;
    private float moveZ;
    public Transform mousePointer;
    public Rigidbody rigi;
    #region ActionList
    public UnityAction<UnityAction> RemoveInput;
    public UnityAction<Vector3> CheckRight;
    public UnityAction<State> ChangeState;
    public UnityAction PlayerKinematic;
    public UnityAction PlayerMove;
    public UnityAction InputCheck;
    #endregion
    [HideInInspector]
    public bool isHit;
    public float invincibilityTime;
    public WaitForSeconds hitDelay;


    private void Awake()
    {
        Setting();
    }

    void Setting()
    {
        hitDelay = new WaitForSeconds(invincibilityTime);
        stateMachine = new StateMachine<State, Player>(this);
        stateMachine.AddState(State.Idle, new PlayerStates.DirectionState());
        stateMachine.AddState(State.Move, new PlayerStates.MoveState());
        stateMachine.AddState(State.Attack, new PlayerStates.AttackState());
        stateMachine.AddState(State.Skill, new PlayerStates.SkiilState());
        stateMachine.AddState(State.Dash,new PlayerStates.DashState());
        stateMachine.AddState(State.Hit,new PlayerStates.HitState());
        #region UnityActionTemplate
        ChangeState += (state) => { stateMachine.ChangeState(state); };
        CheckRight += (target) => { if (transform.position.x < target.x) spriteTransform.localScale = new Vector3(1, 1, 1); else spriteTransform.localScale = new Vector3(-1, 1, 1); };
        PlayerKinematic += () => { rigi.isKinematic = !rigi.isKinematic; };
        PlayerMove += () => { rigi.velocity = moveVec; };
        #endregion
        #region ActionControllerTemplate
        RemoveInput = (actionInput)=>InputCheck -= actionInput;
        InputCheck += MoveInput;
        InputCheck += AttackInput;
        InputCheck += SkillInput;
        InputCheck += DashInput;
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
        if (moveVec == Vector3.zero) curAction = State.Idle;
        else curAction = State.Move;
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

    public override void Hit(IAttackable attacker, Vector3 attackPosition)
    {

    }
    public override void DirectHit()
    {
        rigi.velocity = Vector3.zero;
        rigi.AddForce(crashVec * 30);
    }



}


