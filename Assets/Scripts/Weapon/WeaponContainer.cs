using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class WeaponContainer : MonoBehaviour
{
    public Player player;
    public Weapon curWeapon;
    public Transform weaponVFX;
    public float attackCooltime;
    [HideInInspector]
    public float attackCoolCount = 0;
    [HideInInspector]
    public bool isComboCooltime;
    [HideInInspector]
    public bool superArmor;
    [HideInInspector]
    public int comboCount = 0;
    [HideInInspector]
    public int maxCombo = 0;
    private float afterDelay;
    private float afterDelayCount = 0;
    private float fowardLength;
    private UnityAction afterDelayCounter;
    public UnityAction weaponAttack;
    private Animator playerAnimator;
    private readonly int animator_combo = Animator.StringToHash("Combo");

    private void FixedUpdate()=> afterDelayCounter?.Invoke();

    private void AttackAfterDelay()
    {
        afterDelayCount += Time.fixedDeltaTime;
        if(afterDelayCount>afterDelay)
        {
            weaponVFX.gameObject.SetActive(false);
            afterDelayCount = 0;
            afterDelayCounter -= AttackAfterDelay;
        }
    }

    public void WeaponSet(UnityEngine.AddressableAssets.AssetLabelReference weaponLabel)
    {
        if (curWeapon != null) AddressObject.Release(curWeapon.gameObject);
        curWeapon = AddressObject.Instinate(weaponLabel,weaponVFX).GetComponent<Weapon>();
        curWeapon.transform.localScale = Vector3.zero;
        curWeapon.player = player;
        superArmor = curWeapon.superArmor;
        weaponAttack = curWeapon.WeaponAttack;
        maxCombo = curWeapon.maxCombo;
        playerAnimator.SetInteger(animator_combo, maxCombo);
        afterDelay = curWeapon.afterDelay;
        fowardLength = curWeapon.forwardLength;
        curWeapon.transform.localScale = Vector3.one;
    }

 
    public void WeaponAnimationCancle()
    {
        weaponVFX.gameObject.SetActive(false);
        comboCount = 0;
    }

    public void WeaponAnimationOn()
    {
        if (!weaponVFX.gameObject.activeSelf)
        {
            if (comboCount > maxCombo) comboCount = 0;
            weaponVFX.localPosition = new Vector3(0, 0.5f, ((float)comboCount * fowardLength) + fowardLength);
            weaponVFX.localRotation = Quaternion.Euler(new Vector3(0, 0,comboCount*180));
            weaponVFX.gameObject.SetActive(true);
            afterDelayCounter += AttackAfterDelay;
            weaponAttack?.Invoke();
            comboCount++;
        }
    }


 


}
