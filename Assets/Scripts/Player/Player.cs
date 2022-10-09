using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Player : Character
{
    //private State curDirection;
    public bool isReady { private set; get; }
    public float dashSpeed;
    public float dashCoolTime;
    public float dashInvincibleTime;
    public float invincibleTime;
    private bool isHit;
    private bool isInvicible;
    private bool isDashInvincible;
    private float moveX;
    private float moveZ;
    #region playerParts
    [Header("Parts")]
    [SerializeField]
    private Transform mousePointer;
    [SerializeField]
    private GhostCreator ghostCreator;
    public DirectionCircle directionCircle;
    public WeaponContainer weaponContainer;
    public Animator animator;
    private Rigidbody rigi;
    #endregion
    #region ActionList
    private UnityAction InputCheck;
    private UnityAction CooltimeCounter;
    private UnityAction BehaviorExecute;
    private bool isBehaviorExecuted;
    private bool isAttackBehavior;
    private bool isMoveBehavior;
    private bool isDashBehavior;
    private bool isSkillBehavior;
    private bool isSuperAttackBehavior;
    private bool isIdleBehaviour;
    private bool isDeadBehavior;
    #endregion
    #region Delay&Cooltime
    private float dashCoolCount = 0;
    private float dashInvincibleCount = 0;
    private float invincibleCount = 0;
    public int comboCount = 0;
    public int maxDashable = 1;
    private int _curDashCount = 1;
    public int curDashCount
    {
        get { return _curDashCount; }
        set 
        { 
            _curDashCount = value;
            dashUIcount?.Invoke(value);
        }  
    }
    public UnityAction<int> dashUIcount;
    public UnityAction<float> dashUIcooltime;
    public UnityAction dashUIStart;
    #endregion
    #region PlayerStat
    public UnityAction<float> hpUp;
    public UnityAction<float> maxHpUp; 
    public override float curHp 
    { 
        get => base.curHp;
        set
        {
            if (value > _maxHp) value = _maxHp;
            hpUp?.Invoke(value);
            _curHp = value;
            GameManager.instance.playerInfo.player_curHp = value;
        }
    }
    public override float maxHp 
    {
        get => base.maxHp; 
        set
        {
            if (value > 12) value = 12;
            maxHpUp?.Invoke(value);
            _maxHp = value;
        }
    } 
    #endregion
    protected readonly int animator_Dash = Animator.StringToHash("Dash");
    protected readonly int animator_DashProgress = Animator.StringToHash("DashProgress");
    public readonly int animator_Continue = Animator.StringToHash("Continue");
    public readonly int animator_MotionStop = Animator.StringToHash("MotionStop");
    public Coroutine coUpdate;



    private void Awake()=> rigi = transform.GetComponent<Rigidbody>();
    private void Start() => coUpdate = StartCoroutine(CoUpdate());

    #region Setting
    IEnumerator CoUpdate()
    {
        //PlayerSetting();//Test
        yield return new WaitUntil(() => isReady);
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

    public void  PlayerSetting()
    {
        _maxHp = GameManager.instance.playerInfo.player_maxHp;
        _curHp = GameManager.instance.playerInfo.player_curHp;
        if (GameManager.instance.playerInfo.curEquip != null) ItemManager.instance.PlayerGetWeapon(GameManager.instance.playerInfo.curEquip);
        if (GameManager.instance.playerInfo.inventory.Count != 0)
        {
            foreach (Item item in GameManager.instance.playerInfo.inventory)
            {
                Artifact artifact = item as Artifact;
                if (artifact != null) ItemManager.instance.ArtifactActivation(artifact);
            }
        }
        UIManager.instance.blinkUI?.DashCooltimeSet(this);
        InputCheckSet();
        BehaviorExcuteSet();
        isReady = true;
    }
    public void PlayerSetting(bool isHome)
    {
        if(isHome)
        {
            InputCheck += MoveInput;
            BehaviorExcuteSet();
            isReady = true;
        }
    }




    private void BehaviorExcuteSet()
    {
        BehaviorExecute += PlayerDash;
        BehaviorExecute += PlayerSkill;
        BehaviorExecute += PlayerAttack;
        BehaviorExecute += PlayerMove;
        BehaviorExecute += PlayerIdle;
    }
    public void InputCheckSet()
    {
        InputCheck += MoveInput;
        InputCheck += AttackInput;
        InputCheck += SkillInput;
        InputCheck += DashInput; 
    }
    public void InputCheckRemove() => InputCheck = null;

    #endregion
    #region BehaviorCheck
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
        else isIdleBehaviour = true;
    }
    private void AttackInput()
    {
        if (!Input.GetMouseButton(0)) return;
        if (weaponContainer.weaponAttack == null) return;
        if (weaponContainer.superArmor) isSuperAttackBehavior = true;
        isAttackBehavior = true;
    }
    private void DashInput()
    {
        if(Input.GetMouseButton(1)) isDashBehavior = true;
    }
    private void SkillInput()
    {
    }
    private void ReSetInput()
    {
        isMoveBehavior = false;
        isAttackBehavior = false;
        isDashBehavior=false;
        isSuperAttackBehavior = false;
        isSkillBehavior = false;
        isBehaviorExecuted = false;
        isIdleBehaviour = false;
    }
    private void HitCheck()
    {
        if (isInvicible) return;
        if (isDashInvincible) return;
        if (HitInput == null) return;
        ComboCancle();
        animator.SetTrigger(animator_Hit);
        animator.Update(0);
        HitInput?.Invoke(this);
        isHit = true;
        isInvicible = true;
        CooltimeCounter += InvincibleCount;
    }
    public override bool DeadCheck()
    {
        if(curHp<=0)
        {
            transform.GetComponent<Collider>().enabled = false;
            InputCheck = null;
            BehaviorExecute = null;
            animator.SetTrigger(animator_Dead);
            animator.Update(0);
            StartCoroutine(CoDeadCheck());
        }
        return false;
    }
    private IEnumerator CoDeadCheck()
    {
        yield return new WaitUntil(() => !animator.IsCurStateName(animator_Dead,0));
        GameManager.instance.GameOver();
    }


    #endregion
    #region Behavior
    private void PlayerMove()
    {
        if (isBehaviorExecuted) return;
        if (!isMoveBehavior) return;
        animator.SetBool(animator_Move, true);
        animator.Update(0);
        rigi.velocity = moveVec * moveSpeed;
        isBehaviorExecuted = true;
    }


    private void PlayerIdle()
    {
        if (isBehaviorExecuted) return;
        if (!isIdleBehaviour) return;
        rigi.velocity = Vector3.zero;
        animator.SetBool(animator_Move, false);
        animator.Update(0);
        isBehaviorExecuted = true;
    }

    private void PlayerDash()
    {
        if (isBehaviorExecuted) return; // 필요없음
        if (!isDashBehavior) return;
        if (curDashCount == 0) return;
        gameObject.layer = 11;
        curDashCount--;
        ComboCancle();
        BehaviorExecute += PlayerDashMove;
        InputCheck -= MoveInput;
        InputCheck -= AttackInput;
        InputCheck -= SkillInput;
        InputCheck -= DashInput;
        directionCircle.isStop = true;
        isDashInvincible = true;
        isBehaviorExecuted = true;
        animator.SetTrigger(animator_Dash);
        animator.SetBool(animator_DashProgress, true);
        animator.Update(0);
        ghostCreator.Swith();
        CooltimeCounter += DashInvincibleCount;
    }
    private void PlayerDashMove()
    {
        rigi.velocity = directionCircle.transform.forward * dashSpeed;
    }

    private void PlayerAttack()
    {
        if (isBehaviorExecuted) return;
        if (!isAttackBehavior&&!isSuperAttackBehavior) return;        
        if (weaponContainer.isAttackCooltime) return;
        if (animator.IsInTransition(0)) return;
        rigi.velocity = Vector3.zero;
        if (!animator.IsCurStateTag(animator_Attack, 0))
        {
            directionCircle.isStop = true;
            animator.SetTrigger(animator_Attack);
            animator.Update(0);
            InputCheck -= MoveInput;
            CooltimeCounter += ComboCount;
        }
        else
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f)
            {
                animator.SetBool(animator_Continue,true);
            }
        }
        isBehaviorExecuted = true;
    }
    private void PlayerSkill()
    {
        if (isBehaviorExecuted) return;
        if (!isSkillBehavior) return;
        isBehaviorExecuted = true;
    }

    public override void DirectHit(float damage)
    {
        rigi.velocity = Vector3.zero;
        rigi.AddForce(crashVec * 300);
        curHp -= damage;
    }


    #endregion
    #region CooltimeCounter
    private void DashCoolCount()
    {
        dashCoolCount += Time.fixedDeltaTime;
        dashUIcooltime?.Invoke(dashCoolCount / dashCoolTime);
        if (dashCoolCount >= dashCoolTime)
        {
            CooltimeCounter -= DashCoolCount;
            dashCoolCount = 0;
            curDashCount = maxDashable;
        }
    }
    private void InvincibleCount()
    {
        invincibleCount += Time.fixedDeltaTime;
        if (invincibleCount >= invincibleTime)
        {
            isHit = false;
            isInvicible = false;
            animator.Update(0);
            CooltimeCounter -= InvincibleCount;
            invincibleCount = 0;
            HitInput = null;
        }
    }
    private void DashInvincibleCount()
    {
        dashInvincibleCount += Time.fixedDeltaTime;
        if (dashInvincibleCount >= (dashInvincibleTime+0.5f))
        {
            ghostCreator.Swith();
            rigi.velocity = Vector3.zero;
            animator.SetBool(animator_DashProgress, false);
            animator.Update(0);
            InputCheck += MoveInput;
            InputCheck += AttackInput;
            InputCheck += SkillInput;
            InputCheck += DashInput;
            BehaviorExecute -= PlayerDashMove;
            CooltimeCounter -= DashInvincibleCount;
            if(dashCoolCount == 0)
            {
                dashUIStart?.Invoke();
                CooltimeCounter += DashCoolCount;
            }
            else
            {
                dashCoolCount = 0.02f;
            }
            directionCircle.isStop = false;
            isDashInvincible = false;
            HitInput = null;
            dashInvincibleCount = 0;
            gameObject.layer = 7;
        }
    }
    private void ComboCount()
    {
        if (animator.IsCurStateTag(animator_Attack, 0) || animator.IsInTransition(0)) return;
        ComboEnd();
    }
    private void ComboEnd()
    {
        CooltimeCounter += AttackCooltime;
        CooltimeCounter -= ComboCount;
        InputCheck += MoveInput;
        comboCount = 0;
        directionCircle.isStop = false;
        weaponContainer.isAttackCooltime = true;
    }
    private void ComboCancle()
    {
        if (!animator.IsCurStateTag(animator_Attack, 0)) return;
        animator.SetTrigger(animator_MotionStop);
        animator.Update(0);
        ComboEnd();
    }

    public void AttackCooltime()
    {
        weaponContainer.attackCoolCount += Time.fixedDeltaTime;
        if (weaponContainer.attackCoolCount >= weaponContainer.attackCooltime)
        {
            CooltimeCounter -= AttackCooltime;
            weaponContainer.isAttackCooltime = false;
            weaponContainer.attackCoolCount = 0f;
        }
    }


    #endregion
    public void PlayerKinematic() => rigi.isKinematic = !rigi.isKinematic;





}






