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
    public float duration;
    public float comboDelay = 0;
    public float comboCoolCount = 0;
    public bool isComboCooltime;
    public bool superArmor;
    public int comboCount = 0;


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
    }

    public IEnumerator CoMaterialNoise(bool isDisable)
    {
        float backBaseNoise = 0;
        float addNoise = 1f / duration;
        if(isDisable) {addNoise = -1*addNoise; backBaseNoise = 2;  }
        for (int i = 0; i < duration; i++)
        {
            backBaseNoise -= addNoise;
            containerBackRenderer.transform.localScale = new Vector3(backBaseNoise, backBaseNoise, 1);
            yield return null;
        }
        if (isDisable) containerBackRenderer.transform.localScale = Vector3.zero;
        else containerBackRenderer.transform.localScale = new Vector3(2, 2, 1);
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
            if (comboCount > 2) comboCount = 0;
            weaponVFX.localPosition = new Vector3(-0.3f, 0.5f, (float)comboCount*1f);
            weaponVFX.localRotation = Quaternion.Euler(new Vector3(0, 0, -30+comboCount*30));
            weaponVFX.gameObject.SetActive(true);
            weaponAttack?.Invoke();
            comboCount++;
        }
    }


 


}
