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
    private bool isHit;
    private bool isInvicible;
    private bool isDashInvincible;
    private float moveX;
    private float moveZ;
    #region playerParts
    //[SerializeField]
    //private Transform spriteTransform;
    [SerializeField]
    private Transform mousePointer;
    [SerializeField]
    private GhostCreator ghostCreator;
    [SerializeField]
    private DirectionCircle directionCircle;
    public WeaponContainer weaponContainer;
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
    [SerializeField]
    private float dashCoolTime;
    private float dashCoolCount = 0;
    [SerializeField]
    private float dashInvincibleTime;
    [SerializeField]
    private float dashInvincibleCount = 0;
    [SerializeField]
    private float invincibleTime;
    private float invincibleCount = 0;
    private float comboTime = 0.4f;
    private float comboCount = 0f;
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
        if(/*SceneManager.GetActiveScene().name== "GameScene"*/true)
        {
            yield return WaitList.isSingletonSet;
            _maxHp = GameManager.instance.playerInfo.player_maxHp;
            _curHp = GameManager.instance.playerInfo.player_curHp;
            if (GameManager.instance.playerInfo.curEquip != null) ItemManager.instance.PlayerGetWeapon(GameManager.instance.playerInfo.curEquip);
            if (GameManager.instance.playerInfo.inventory.Count != 0)
            {
                foreach (Item item in GameManager.instance.playerInfo.inventory)
                {
                    Artifact artifact = item as Artifact;
                    if (artifact != null) ItemManager.instance.PlayerGetArtifact(artifact);
                }
            }
            InputCheckSet();
        }
        else
        {
            InputCheck += MoveInput;
            InputCheck += AttackInput;
            InputCheck += DashInput;
        }
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
        if (!Input.GetMouseButton(0)) { weaponContainer.WeaponAnimationOff(); return; }
        if (weaponContainer.weaponAttack == null) { AttackBehavior = null; return; }
        if (weaponContainer.superArmor) isSuperAttackBehavior = true;
        //AttackBehavior = weaponContainer.WeaponAnimationOn;
        AttackBehavior = weaponContainer.TestTemp;
        isAttackBehavior = true;
    }
    private void DashInput()
    {
        if(Input.GetMouseButton(1)) isDashBehavior = true;
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
        isIdleBehaviour = false;
    }
    private void HitCheck()
    {
        if (isInvicible) return;
        if (isDashInvincible) return;
        if (HitInput == null) return;
        weaponContainer.WeaponAnimationOff();
        animator.SetTrigger("MotionStop");
        HitInput?.Invoke(this);
        isHit = true;
        isInvicible = true;
        CooltimeCounter += InvincibleCount;
    }
    #endregion
    #region Behavior
    private void PlayerMove()
    {
        if (isBehaviorExecuted) return;
        if (!isMoveBehavior) return;
        animator.SetBool("Move",true);
        rigi.velocity = moveVec * moveSpeed;
        isBehaviorExecuted = true;
    }

    private void PlayerIdle()
    {
        if (isBehaviorExecuted) return;
        if (!isIdleBehaviour) return;
        rigi.velocity = Vector3.zero;
        animator.SetBool("Move", false);
        isBehaviorExecuted = true;
    }

    private void PlayerDash()
    {
        if (isBehaviorExecuted) return; // 필요없음
        if (!isDashBehavior) return;
        AttackComboCancle();
        rigi.velocity = (mousePointer.position - transform.position).normalized * dashSpeed;
        InputCheck -= MoveInput;
        InputCheck -= AttackInput;
        InputCheck -= SkillInput;
        InputCheck -= DashInput;
        directionCircle.isStop = true;
        isDashInvincible = true;
        isBehaviorExecuted = true;
        animator.SetBool("Dash", true);
        ghostCreator.Swith();
        CooltimeCounter += DashCoolCount;
        CooltimeCounter += DashInvincibleCount;
    }
    private void PlayerAttack()
    {
        if (isBehaviorExecuted) return;
        if (!isAttackBehavior&&!isSuperAttackBehavior) return;
        rigi.velocity = Vector3.zero;
        if (!weaponContainer.isProgress)
        {
            animator.SetBool("Attack", true);
            InputCheck -= MoveInput;
            isComboBehaviour = true;
            CooltimeCounter += ComboCount;
        }
        else comboCount = 0;
        if(weaponContainer.visualEffect.aliveParticleCount!=0) AttackBehavior?.Invoke();
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
            ghostCreator.Swith();
            animator.SetBool("Dash", false);
            directionCircle.isStop = false;
            HitInput = null;
            dashInvincibleCount = 0;
        }
    }
    private void ComboCount()
    {
        comboCount += 0.02f;
        if(comboCount>=comboTime||!weaponContainer.isProgress)
        {       
            weaponContainer.WeaponAnimationOff();
            ComboEnd();
        }
    }
    private void ComboEnd()
    {
        animator.SetBool("Attack", false);
        animator.Update(0f);
        isComboBehaviour = false;
        CooltimeCounter -= ComboCount;
        InputCheck += MoveInput;
        comboCount = 0;
    }


    #endregion
    public void PlayerKinematic() => rigi.isKinematic = !rigi.isKinematic;
    private void AttackComboCancle()
    {
        if (!weaponContainer.isProgress) return;
        weaponContainer.WeaponAnimationOffUpdate();
        if (!isComboBehaviour) return;
        ComboEnd();
    }
    //private void RightCheck()
    //{
    //    if (transform.position.x < mousePointer.position.x) { spriteTransform.localScale = new Vector3(1, 1, 1); }
    //    else { spriteTransform.localScale = new Vector3(-1, 1, 1); }
    //}
    




}






