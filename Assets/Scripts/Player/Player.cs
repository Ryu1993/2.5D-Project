using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Player : Character
{
    private State[] directionsIdle = { State.Up, State.Down, State.Horizontal, State.HoUp, State.HoDown, State.Idle };
    private State curDirection;
    private StateMachine<State, Player> stateMachine;
    [SerializeField]
    private Transform spriteTransform;
    public ParticleSystem illusionCreator;
    private float moveX;
    private float moveZ;
    public WeaponContainer weaponContainer;
    public Transform mousePointer;
    public DirectionCircle directionCircle;
    #region ActionList
    public UnityAction InputCheck;
    public UnityAction AttackBehavior;
    public bool isAttackBehavior;
    public bool isMoveBehavior;
    public bool isDashBehavior;
    public bool isSkillBehavior;
    public bool isSuperAttackBehavior;
    public bool isIdleBehvior;
    #endregion
    [HideInInspector]
    public bool isHit;
    [SerializeField]
    float _invincibilityTime;
    float invincibilityTime
    {
        get { return _invincibilityTime; }
        set
        { 
            _invincibilityTime = value;
            hitDelay = new WaitForSeconds(invincibilityTime);
        }
    }
    [SerializeField]
    float _dashTime;
    float dashTime
    {
        get { return _dashTime; }
        set
        { 
            _dashTime = value;
            dashDelay = new WaitForSeconds(dashTime);
        }         
    }
    public WaitForSeconds dashDelay;
    public WaitForSeconds hitDelay;
    public WaitForFixedUpdate fixedUpdateDelay = new WaitForFixedUpdate();
    private void Awake()
    {
        Setting();
    }
    private void Update()
    {
        InputCheck();
    }

    void Setting()
    {
        dashDelay = new WaitForSeconds(dashTime);
        hitDelay = new WaitForSeconds(invincibilityTime);
        stateMachine = new StateMachine<State, Player>(this);
        stateMachine.AddState(State.Idle, new PlayerStates.DirectionState());
        stateMachine.AddState(State.Move, new PlayerStates.MoveState());
        stateMachine.AddState(State.Attack, new PlayerStates.AttackState());
        stateMachine.AddState(State.SuperAttack, new PlayerStates.SuperAttackState());
        stateMachine.AddState(State.Skill, new PlayerStates.SkiilState());
        stateMachine.AddState(State.Dash, new PlayerStates.DashState());
        stateMachine.AddState(State.Hit, new PlayerStates.HitState());
        #region ActionControllerTemplate
        InputCheck += MoveInput;
        InputCheck += AttackInput;
        InputCheck += SkillInput;
        InputCheck += DashInput;
        #endregion
        curAction = State.Idle;
        curDirection = State.Up;
        ChangeState(State.Idle);
    }
    public State DirectionState()
    {
        RightCheck();
        float dot = Vector3.Dot(transform.forward, (mousePointer.position - transform.position).normalized);
        if (dot > 0.866) curDirection = State.Up;
        else if (dot > 0.5) curDirection = State.HoUp;
        else if (dot > -0.5) curDirection = State.Horizontal;
        else if (dot > -0.866) curDirection = State.HoDown;
        else if(dot>-1)curDirection = State.Down;
        return curDirection;
    }
    public void MoveInput()
    {
        isIdleBehvior = false;
        isMoveBehavior = false;
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        moveVec = new Vector3(moveX, 0, moveZ);
        if (moveVec == Vector3.zero) isIdleBehvior = true;
        else { moveVec = new Vector3(moveX, 0, moveZ).normalized * moveSpeed;isMoveBehavior = true; }      
    }
    public void AttackInput()
    {
        isAttackBehavior = false;
        if (!Input.GetMouseButton(0)) { weaponContainer.WeaponAnimationOff(); return; }
        if (weaponContainer.weaponAttack == null) { AttackBehavior = null; return; }
        AttackBehavior = weaponContainer.WeaponAnimationOn;
        isAttackBehavior = true;
    }
    public void DashInput()
    {
        isDashBehavior = false;
        if (!Input.GetMouseButton(1)) return;
        isDashBehavior = true;
    }
    public void SkillInput()
    {
    }

    public void HitReset()=> HitInput = null;
    public void RemoveInput(UnityAction inputAction) => InputCheck -= inputAction;
    public void PlayerDash(float speed)=>rigi.velocity = directionCircle.transform.forward * speed;
    public void PlayerMove() => rigi.velocity = moveVec;
    public void PlayerDashMove(float speed) => rigi.velocity = moveVec * speed;
    public void PlayerIdle() => rigi.velocity = Vector3.zero;
    public void PlayerKinematic()=> rigi.isKinematic = !rigi.isKinematic;
    public void ChangeState(State state) => stateMachine.ChangeState(state);
    private void RightCheck()
    {
        if (transform.position.x < mousePointer.position.x) { spriteTransform.localScale = new Vector3(1, 1, 1); }
        else { spriteTransform.localScale = new Vector3(-1, 1, 1); }
    }






}






