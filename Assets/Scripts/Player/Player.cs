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
    public float comboTime = 0.5f;
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
    [SerializeField]
    private DirectionCircle directionCircle;
    public WeaponContainer weaponContainer;
    public Animator animator;
    private Rigidbody rigi;
    #endregion
    #region ActionList
    private UnityAction InputCheck;
    private UnityAction AttackBehavior;
    private UnityAction CooltimeCounter;
    private UnityAction BehaviorExecute;
    private bool isBehaviorExecuted;
    private bool isAttackBehavior;
    private bool isComboBehaviour;
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
    private float comboCount = 0f;
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
    public readonly int animator_Dash = Animator.StringToHash("Dash");
    public Coroutine coUpdate;


    private void Awake()=> rigi = transform.GetComponent<Rigidbody>();
    private void Start() => coUpdate = StartCoroutine(CoUpdate());

    #region Setting
    IEnumerator CoUpdate()
    {
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
        InputCheckSet();
        BehaviorExcuteSet();
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
        if (weaponContainer.weaponAttack == null) { AttackBehavior = null; return; }
        if (weaponContainer.superArmor) isSuperAttackBehavior = true;
        AttackBehavior = weaponContainer.WeaponAnimationOn;
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
        yield return new WaitUntil(() => !animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"));
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
        gameObject.layer = 11;
        ComboCancle();
        rigi.velocity = (mousePointer.position - transform.position).normalized * dashSpeed;
        InputCheck -= MoveInput;
        InputCheck -= AttackInput;
        InputCheck -= SkillInput;
        InputCheck -= DashInput;
        directionCircle.isStop = true;
        isDashInvincible = true;
        isBehaviorExecuted = true;
        animator.SetBool(animator_Dash, true);
        ghostCreator.Swith();
        CooltimeCounter += DashCoolCount;
        CooltimeCounter += DashInvincibleCount;
    }
    private void PlayerAttack()
    {
        if (isBehaviorExecuted) return;
        if (!isAttackBehavior&&!isSuperAttackBehavior) return;
        if (weaponContainer.isComboCooltime) return;
        rigi.velocity = Vector3.zero;
        if (!isComboBehaviour)
        {
            animator.SetBool(animator_Attack, true);
            InputCheck -= MoveInput;
            CooltimeCounter += ComboCount;
            isComboBehaviour = true;
        }
        else comboCount = 0;
        AttackBehavior?.Invoke();
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
        Debug.Log(damage);
        curHp -= damage;
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
            animator.Update(0);
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
            animator.SetBool(animator_Dash, false);
            directionCircle.isStop = false;
            InputCheck += MoveInput;
            InputCheck += AttackInput;
            InputCheck += SkillInput;
            CooltimeCounter -= DashInvincibleCount;
            ghostCreator.Swith();
            HitInput = null;
            dashInvincibleCount = 0;
            gameObject.layer = 7;
        }
    }
    private void ComboCount()
    {
        comboCount += 0.02f;
        if(comboCount>=comboTime||weaponContainer.comboCount>=weaponContainer.maxCombo) ComboEnd();
    }
    private void ComboEnd()
    {
        animator.SetBool(animator_Attack, false);
        animator.Update(0f);
        CooltimeCounter += AttackCooltime;
        CooltimeCounter -= ComboCount;
        InputCheck += MoveInput;
        weaponContainer.isComboCooltime = true;
        isComboBehaviour = false;
        weaponContainer.comboCount = 0;
        comboCount = 0;
    }
    private void ComboCancle()
    {
        if (!weaponContainer.weaponVFX.gameObject.activeSelf) return;
        weaponContainer.WeaponAnimationCancle();
        if (!isComboBehaviour) return;
        ComboEnd();
    }

    public void AttackCooltime()
    {
        weaponContainer.attackCoolCount += 0.02f;
        if (weaponContainer.attackCoolCount >= weaponContainer.attackCooltime)
        {
            CooltimeCounter -= AttackCooltime;
            weaponContainer.isComboCooltime = false;
            weaponContainer.attackCoolCount = 0f;
        }

    }




    #endregion
    public void PlayerKinematic() => rigi.isKinematic = !rigi.isKinematic;




}






