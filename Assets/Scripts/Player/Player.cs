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
    public float comboTime = 0.5f;
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
                if (artifact != null) ItemManager.instance.PlayerGetArtifact(artifact);
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
        ComboCancle();
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
        if (isBehaviorExecuted) return; // �ʿ����
        if (!isDashBehavior) return;
        ComboCancle();
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
        if (weaponContainer.isComboCooltime) return;
        rigi.velocity = Vector3.zero;
        if (!isComboBehaviour)
        {
            animator.SetBool("Attack", true);
            weaponContainer.StartCoroutine(weaponContainer.CoMaterialNoise(true));
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
    #endregion
    #region CooltimeCounter

    public override void DirectHit(float damage)
    {
        rigi.velocity = Vector3.zero;
        rigi.AddForce(crashVec * 30);
        curHp -= damage;
    }
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
        if(comboCount>=comboTime||weaponContainer.comboCount>=3) ComboEnd();
    }
    private void ComboEnd()
    {
        animator.SetBool("Attack", false);
        animator.Update(0f);
        StartCoroutine(weaponContainer.CoMaterialNoise(false));
        CooltimeCounter += ComboCooltime;
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

    public void ComboCooltime()
    {
        weaponContainer.comboCoolCount += 0.02f;
        if (weaponContainer.comboCoolCount >= weaponContainer.comboDelay)
        {
            CooltimeCounter -= ComboCooltime;
            weaponContainer.isComboCooltime = false;
            weaponContainer.comboCoolCount = 0f;
        }

    }




    #endregion
    public void PlayerKinematic() => rigi.isKinematic = !rigi.isKinematic;




}






