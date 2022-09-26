using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Player : Character
{
    private State curDirection;
    public bool isReady { private set; get; }
    private bool isHit;
    private bool isInvicible;
    private bool isDashInvincible;
    private float moveX;
    private float moveZ;
    #region playerParts
    [SerializeField]
    private Transform spriteTransform;
    [SerializeField]
    private Transform mousePointer;
    [SerializeField]
    private ParticleSystem illusionCreator;
    [SerializeField]
    private DirectionCircle directionCircle;
    public WeaponContainer weaponContainer;
    #endregion
    #region ActionList
    private UnityAction InputCheck;
    private UnityAction AttackBehavior;
    private UnityAction CooltimeCounter;
    private UnityAction BehaviorExecute;
    [SerializeField]
    private bool isBehaviorExecuted;
    [SerializeField]
    private bool isAttackBehavior;
    [SerializeField]
    private bool isMoveBehavior;
    [SerializeField]
    private bool isDashBehavior;
    [SerializeField]
    private bool isSkillBehavior;
    [SerializeField]
    private bool isSuperAttackBehavior;
    private bool isDeadBehavior;
    #endregion
    #region Delay&Cooltime
    [SerializeField]
    private float dashCoolTime;
    private float dashCoolCount = 0;
    [SerializeField]
    private float dashInvincibleTime;
    private float dashInvincibleCount = 0;
    [SerializeField]
    private float invincibleTime;
    private float invincibleCount = 0;
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
    public Coroutine coUpdate;
    #endregion


    private void Awake() => StartCoroutine(CoSetting());
    private void Start() => coUpdate = StartCoroutine(CoUpdate());

    #region Setting
    IEnumerator CoUpdate()
    {
        while(true)
        {
            CooltimeCounter?.Invoke();
            ReSetInput();
            HitCheck();
            if (!isHit)
            {
                InputCheck?.Invoke();
                BehaviorExecute?.Invoke();
            }
            yield return WaitList.fixedUpdate;
        }
    }

    IEnumerator CoSetting()
    {
        yield return WaitList.isGameManagerSet;
        yield return WaitList.isSingletonSet;
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
        InputCheckSet();
        BehaviorExcuteSet();
        curDirection = State.Up;
        isReady = true;
    }

    private void BehaviorExcuteSet()
    {
        BehaviorExecute += PlayerDash;
        BehaviorExecute += PlayerSkill;
        BehaviorExecute += PlayerAttack;
        BehaviorExecute += PlayerMove;
        BehaviorExecute += PlayerIdle;
    }
    private void InputCheckSet()
    {
        InputCheck += MoveInput;
        InputCheck += AttackInput;
        InputCheck += SkillInput;
        InputCheck += DashInput;
        InputCheck += DeadInput;     
    }
    #endregion
    #region BehaviorCheck
    private void DirectionState()
    {
        RightCheck();
        float dot = Vector3.Dot(transform.forward, (mousePointer.position - transform.position).normalized);
        if (dot > 0.866) curDirection = State.Up;
        else if (dot > 0.5) curDirection = State.HoUp;
        else if (dot > -0.5) curDirection = State.Horizontal;
        else if (dot > -0.866) curDirection = State.HoDown;
        else if (dot > -1) curDirection = State.Down;
    }
    private void MoveInput()
    {
        moveX = Input.GetAxis("Horizontal");
        moveZ = Input.GetAxis("Vertical");
        moveVec = new Vector3(moveX, 0, moveZ);
        if (moveVec != Vector3.zero)
        {
            moveVec = new Vector3(moveX, 0, moveZ).normalized;
            isMoveBehavior = true;
        }

    }
    private void AttackInput()
    {
        if (!Input.GetMouseButton(0)) { weaponContainer.WeaponAnimationOff(); return; }
        if (weaponContainer.weaponAttack == null) { AttackBehavior = null; return; }
        if (weaponContainer.superArmor) isSuperAttackBehavior = true;
        AttackBehavior = weaponContainer.WeaponAnimationOn;
        isAttackBehavior = true;
    }
    private void DashInput()
    {
        if(Input.GetMouseButton(1))
        {
            isDashBehavior = true;
        }
    }
    private void SkillInput()
    {
    }
    private void DeadInput() => isDeadBehavior = curHp <= 0;
    private void ReSetInput()
    {
        isMoveBehavior = false;
        isAttackBehavior = false;
        isDashBehavior=false;
        isSuperAttackBehavior = false;
        isSkillBehavior = false;
        isBehaviorExecuted = false;
    }
    private void HitCheck()
    {
        if (isInvicible) return;
        if (isDashInvincible) return;
        if (HitInput == null) return;
        if(isSkillBehavior||isSuperAttackBehavior)
        {
            weaponContainer.WeaponAnimationOff();
            animator.SetTrigger("MotionStop");
        }
        HitInput?.Invoke(this);
        isHit = true;
        isInvicible = true;
        InputCheck -= MoveInput;
        InputCheck -= AttackInput;
        InputCheck -= SkillInput;
        CooltimeCounter += InvincibleCount;
    }
    #endregion
    #region Behavior
    private void PlayerMove()
    {
        if (isBehaviorExecuted) return;
        if (!isMoveBehavior) return;
        DirectionState();
        animator.SetTrigger("Move");
        rigi.velocity = moveVec * moveSpeed;
        isBehaviorExecuted = true;
    }

    private void PlayerIdle()
    {
        if (isBehaviorExecuted) return;
        DirectionState();
        rigi.velocity = Vector3.zero;
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) animator.SetTrigger("MotionStop");
        isBehaviorExecuted = true;
    }

    private void PlayerDash()
    {
        if (isBehaviorExecuted) return; // 필요없음
        if (!isDashBehavior) return;
        AttackComboCancle();
        InputCheck -= MoveInput;
        InputCheck -= AttackInput;
        InputCheck -= SkillInput;
        InputCheck -= DashInput;
        rigi.velocity = directionCircle.transform.forward * moveSpeed * 2;
        isDashInvincible = true;
        CooltimeCounter += DashCoolCount;
        CooltimeCounter += DashInvincibleCount;
        isBehaviorExecuted = true;
    }
    private void PlayerAttack()
    {
        if (isBehaviorExecuted) return;
        if (!isAttackBehavior&&!isSuperAttackBehavior) return;
        AttackBehavior?.Invoke();
        isBehaviorExecuted = true;
    }
    private void PlayerSkill()
    {
        if (isBehaviorExecuted) return;
        if (!isSkillBehavior) return;
        isBehaviorExecuted = true;
    }
    #endregion
    #region CooltimeCounter
    private void DashCoolCount()
    {
        dashCoolCount += 0.02f;
        if (dashCoolCount >= dashCoolTime)
        {
            InputCheck += DashInput;
            CooltimeCounter -= DashCoolCount;
            dashCoolCount = 0;
        }
    }
    private void InvincibleCount()
    {
        invincibleCount += 0.02f;
        if(invincibleCount >= invincibleTime)
        {
            isHit = false;
            isInvicible = false;
            InputCheck += MoveInput;
            InputCheck += AttackInput;
            InputCheck += SkillInput;
            CooltimeCounter -= InvincibleCount;
            invincibleCount = 0;
            HitInput = null;
        }
    }
    private void DashInvincibleCount()
    {
        dashInvincibleCount += 0.02f;
        if (dashInvincibleCount >= dashInvincibleTime)
        {
            isDashInvincible = false;
            InputCheck += MoveInput;
            InputCheck += AttackInput;
            InputCheck += SkillInput;
            CooltimeCounter -= DashInvincibleCount;
            dashInvincibleCount = 0;
            HitInput = null;
        }
    }
    #endregion
    public void PlayerKinematic() => rigi.isKinematic = !rigi.isKinematic;
    private void AttackComboCancle()
    {
        if (!weaponContainer.isProgress) return;
        weaponContainer.WeaponAnimationOff();
    }
    private void RightCheck()
    {
        if (transform.position.x < mousePointer.position.x) { spriteTransform.localScale = new Vector3(1, 1, 1); }
        else { spriteTransform.localScale = new Vector3(-1, 1, 1); }
    }
    




}






