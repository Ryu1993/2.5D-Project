using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class WeaponContainer : MonoBehaviour
{
    private readonly int desolve = Shader.PropertyToID("_Desolve");
    public Player player;
    public Weapon curWeapon;
    [SerializeField]
    private SpriteRenderer containerBackRenderer;
    public Transform attackPoint;
    public Transform weaponVFX;
    public Transform weaponSlot;
    public UnityAction weaponAttack;
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

    public void WeaponSet(UnityEngine.AddressableAssets.AssetLabelReference weaponLabel)
    {
        if (curWeapon != null) AddressObject.Release(curWeapon.gameObject);
        curWeapon = AddressObject.Instinate(weaponLabel,weaponSlot).GetComponent<Weapon>();
        curWeapon.transform.localScale = Vector3.zero;
        containerBackRenderer.sprite = curWeapon.sprite;
        curWeapon.player = player;
        curWeapon.attackPoint = attackPoint;
        superArmor = curWeapon.superArmor;
        weaponAttack = curWeapon.WeaponAttack;
        maxCombo = curWeapon.maxCombo;
    }

 
    public void WeaponAnimationCancle()
    {
        weaponVFX.gameObject.SetActive(false);
        containerBackRenderer.material.SetFloat(desolve, 0);
        comboCount = 0;
    }

    public void WeaponAnimationOn()
    {
        if (!weaponVFX.gameObject.activeSelf)
        {
            if (comboCount > maxCombo) comboCount = 0;
            weaponVFX.localPosition = new Vector3(-0.3f, 0.5f, (float)comboCount*1f);
            weaponVFX.localRotation = Quaternion.Euler(new Vector3(0, 0,comboCount*180));
            weaponVFX.gameObject.SetActive(true);
            weaponAttack?.Invoke();
            comboCount++;
        }
    }


 


}
