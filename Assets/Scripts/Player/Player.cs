using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Player : Character
{
    private State curDirection;
    private StateMachine<State, Player> stateMachine;
    [HideInInspector]
    public bool isHit;
    public bool isReady;
    private float moveX;
    private float moveZ;
    #region playerParts
    [SerializeField]
    private Transform spriteTransform;
    public ParticleSystem illusionCreator;
    public WeaponContainer weaponContainer;
    public Transform mousePointer;
    public DirectionCircle directionCircle;
    #endregion
    #region ActionList
    public UnityAction InputCheck;
    public UnityAction AttackBehavior;
    public UnityAction CooltimeCounter;
    public UnityAction BehaviorExecute;
    public bool isAttackBehavior;
    public bool isMoveBehavior;
    public bool isDashBehavior;
    public bool isSkillBehavior;
    public bool isSuperAttackBehavior;
    public bool isIdleBehavior;
    public bool isDeadBehavior;

    #endregion
    #region delay
    [SerializeField]
    private int dashCoolFrame;
    private int dashCoolCount = 0;

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
    #endregion
    #region PlayerStat
    public UnityAction<int> hpUp;
    public UnityAction<int> maxHpUp;
    public override float curHp 
    { 
        get => base.curHp;
        set
        {
            if (value > _maxHp) value = _maxHp;
            hpUp?.Invoke((int)(value - _curHp));
            _curHp = value;
        }
    }
    public override float maxHp 
    {
        get => base.maxHp; 
        set
        {
            if (value > 12) value = 12;
            maxHpUp?.Invoke((int)(value - _maxHp));
            _maxHp = value;
        }
    }
    #endregion
    private void Awake()
    {
        StartCoroutine(CoSetting());
    }
    private void FixedUpdate()
    {
        CooltimeCounter?.Invoke();
        RemoveInput();
        HitCheck();
        InputCheck?.Invoke();
        BehaviorExecute?.Invoke();
    }


    IEnumerator CoSetting()
    {
        int maximum = 300;
        int count = 0;
        while (true)
        {
            if (count > maximum)
            {
                Debug.Log("세팅실패"); 
                yield break;
            }
            if(GameManager.instance != null)
            {
                if (GameManager.instance.isSetComplete) break;
            }
            count++;
            yield return null;
        }
        _maxHp = GameManager.instance.playerInfo.player_maxHp;
        _curHp = GameManager.instance.playerInfo.player_curHp;
        if(GameManager.instance.playerInfo.curEquip!=null) ItemManager.instance.PlayerGetWeapon(GameManager.instance.playerInfo.curEquip);
        if(GameManager.instance.playerInfo.inventory.Count!=0)
        {
            foreach (Item item in GameManager.instance.playerInfo.inventory)
            {
                Artifact artifact = item as Artifact;
                if(artifact!=null) ItemManager.instance.PlayerGetArtifact(artifact);
            }
        }
        Debug.Log("아티팩트완료");
        dashDelay = new WaitForSeconds(dashTime);
        hitDelay = new WaitForSeconds(invincibilityTime);
        stateMachine = new StateMachine<State, Player>(this);
        //stateMachine.AddState(State.Idle, new PlayerStates.DirectionState());
        //stateMachine.AddState(State.Move, new PlayerStates.MoveState());
        //stateMachine.AddState(State.Attack, new PlayerStates.AttackState());
        //stateMachine.AddState(State.SuperAttack, new PlayerStates.SuperAttackState());
        //stateMachine.AddState(State.Skill, new PlayerStates.SkiilState());
        //stateMachine.AddState(State.Dash, new PlayerStates.DashState());
        //stateMachine.AddState(State.Hit, new PlayerStates.HitState());
        #region ActionControllerTemplate
        InputCheck += MoveInput;
        InputCheck += AttackInput;
        InputCheck += SkillInput;
        InputCheck += DashInput;
        InputCheck += DeadInput;
        #endregion
        curAction = State.Idle;
        curDirection = State.Up;
        isReady = true;
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
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        moveVec = new Vector3(moveX, 0, moveZ);
        if (moveVec == Vector3.zero) isIdleBehavior = true;
        else { moveVec = new Vector3(moveX, 0, moveZ).normalized;isMoveBehavior = true; }      
    }
    public void AttackInput()
    {
        if (!Input.GetMouseButton(0)) { weaponContainer.WeaponAnimationOff(); return; }
        if (weaponContainer.weaponAttack == null) { AttackBehavior = null; return; }
        AttackBehavior = weaponContainer.WeaponAnimationOn;
        isAttackBehavior = true;
    }
    public void AttackStop()
    {
        weaponContainer.WeaponAnimationOff();
        AttackBehavior = null;
    }
    public void DashInput()
    {
        if(Input.GetMouseButton(1))
        {
            isDashBehavior = true;
        }
    }
    public void SkillInput()
    {
    }
    public void HitReset()=> HitInput = null;
    public void DeadInput() => isDeadBehavior = curHp <= 0;
    public void RemoveInput()
    {
        isMoveBehavior = false;
        isAttackBehavior = false;
        isIdleBehavior = false;
        isDashBehavior=false;
        isSuperAttackBehavior = false;
        isSkillBehavior = false;
    }
    public void HitCheck()
    {
        

    }
    public void PlayerMove() => rigi.velocity = moveVec * moveSpeed;
    public void PlayerDashMove() => rigi.velocity = moveVec.normalized * moveSpeed * 2;
    public void PlayerDash() => rigi.velocity = directionCircle.transform.forward * moveSpeed * 2;
    public void PlayerIdle() => rigi.velocity = Vector3.zero;
    public void PlayerKinematic()=> rigi.isKinematic = !rigi.isKinematic;
    public void ChangeState(State state) => stateMachine.ChangeState(state);
    public void DashCoolCount()
    {
        InputCheck -= DashInput;
        dashCoolCount++;
        if (dashCoolCount >= dashCoolFrame)
        {
            InputCheck += DashInput;
            CooltimeCounter -= DashCoolCount;
            dashCoolCount = 0;
        }
    }
    public void InvincibleCount()
    {

    }
    private void RightCheck()
    {
        if (transform.position.x < mousePointer.position.x) { spriteTransform.localScale = new Vector3(1, 1, 1); }
        else { spriteTransform.localScale = new Vector3(-1, 1, 1); }
    }
    




}






