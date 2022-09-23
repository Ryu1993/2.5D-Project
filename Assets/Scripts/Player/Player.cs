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
            hpUp?.Invoke((int)(value - _curHp));
            if (value > _maxHp) _curHp = _maxHp;
            _curHp = value;
        }
    }
    public override float maxHp 
    {
        get => base.maxHp; 
        set
        {
            maxHpUp?.Invoke((int)(value - _maxHp));
            if (value > 12) value = 12;
            _maxHp = value;
        }
    }
    #endregion
    private void Awake()
    {
        StartCoroutine(CoSetting());
    }
    private void Update()
    {
        InputCheck?.Invoke();
    }

    IEnumerator CoSetting()
    {
        int maximum = 300;
        int count = 0;
        bool isFaile = false;
        while (true)
        {
            if (count > maximum)
            {
                isFaile = true;
                break;
            }
            if(GameManager.instance != null)
            {
                if (GameManager.instance.isSetComplete) break;
            }
            count++;
            yield return null;
        }
        if (isFaile)
        {
            Debug.Log("GameManagerLoadingFaile"); yield break;
        }
        _maxHp = GameManager.instance.playerInfo.player_maxHp;
        _curHp = GameManager.instance.playerInfo.player_curHp;
        if(GameManager.instance.playerInfo.curEquip!=null)
        {
            GameObject go = AddressObject.Instinate(GameManager.instance.playerInfo.curEquip.weaponPrefab, weaponContainer.weaponSlot);
            weaponContainer.WeaponSet(go.GetComponent<Weapon>());
            while(UIManager.instance==null)
            {
                yield return null;
            }
            UIManager.instance.weaponUI.ChangeWeapon(GameManager.instance.playerInfo.curEquip);
            Debug.Log("무기완료");
        }
        foreach(Item item in GameManager.instance.playerInfo.inventory)
        {
            Artifact artifact = item as Artifact;
            artifact?.GetArtifactEvent(this);
            
        }
        Debug.Log("아티팩트완료");
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
        isIdleBehavior = false;
        isMoveBehavior = false;
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        moveVec = new Vector3(moveX, 0, moveZ);
        if (moveVec == Vector3.zero) isIdleBehavior = true;
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
        if(Input.GetMouseButton(1))
        {
            Debug.Log("키 받음");
            isDashBehavior = true;
        }
    }
    public void SkillInput()
    {
    }
    public void HitReset()=> HitInput = null;
    public void DeadInput() => isDeadBehavior = curHp <= 0;
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






