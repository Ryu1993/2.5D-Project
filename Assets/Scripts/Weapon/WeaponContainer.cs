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
    public bool isAttackCooltime;
    [HideInInspector]
    public bool superArmor;
    [HideInInspector]
    public int comboCount = 0;
    [HideInInspector]
    public int maxCombo = 0;
    public UnityAction<int> weaponAttack;
    public readonly int animator_combo = Animator.StringToHash("Combo");

    public void WeaponSet(Equip equip)
    {
        if (curWeapon != null) DestroyImmediate(curWeapon.gameObject);
        weaponVFX.localPosition = new Vector3(0, 0.8f, 0.8f);
        Instantiate(equip.itemPrefab,weaponVFX).TryGetComponent(out curWeapon);
        WeaponStatusSet(equip);
        superArmor = curWeapon.superArmor;
        maxCombo = curWeapon.maxCombo;
        attackCooltime = curWeapon.afterDelay;
        weaponAttack = curWeapon.WeaponAttack;
        player.animator.SetInteger(animator_combo, maxCombo);
        curWeapon.transform.localScale = Vector3.one;
    }

    public void WeaponStatusSet(Equip equip)
    {
        curWeapon.player = player;
        curWeapon.damage = equip.damage;
        curWeapon.attackRange = equip.attackRange;
        curWeapon.afterDelay = equip.attackCooltime;
        curWeapon.superArmor = equip.isSuperArmor;
        curWeapon.maxCombo = equip.maxCombo;
    }

}
