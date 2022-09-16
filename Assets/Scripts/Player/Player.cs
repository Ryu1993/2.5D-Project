using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Player : Singleton<Player> , IDamageable
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
    private Vector3 moveVec;
    private Vector3 crashVec;
    private float moveX;
    private float moveZ;
    private State curDirection;
    public UnityAction<State> ChangeState;
    public UnityAction PlayerKinematic;
    public UnityAction PlayerMove;
    private UnityAction<Vector3> CheckRight;
    public UnityAction InputCheck;
    public UnityAction<UnityAction> RemoveInput;
    public UnityAction<IDamageable> HitInput;
    [HideInInspector]
    public State curAction;
    [HideInInspector]
    public bool isHit;
    public float invincibilityTime;
    public WaitForSeconds hitDelay;


    protected override void Awake()
    {
        base.Awake();
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



    public void Hit()
    {
        rigi.velocity = Vector3.zero;
        rigi.AddForce(crashVec * 30);
    }
    public void DotHit(IAttackable.DotType type)
    {

    }

    public void CrowdControlHit(IAttackable.CCType type)
    {

    }

    public void OnCollisionEnter(Collision collision)
    {
        if(isHit)
        {
            return;
        }
        IAttackable crashgo = collision.transform.GetComponent<IAttackable>();
        if(crashgo!=null)
        {
            HitInput = crashgo.Attack;
            isHit = true;
            crashVec = -1 * (collision.transform.position - transform.position).normalized;
        }
    }
}


