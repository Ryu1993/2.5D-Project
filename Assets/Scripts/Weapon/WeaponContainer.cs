using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class WeaponContainer : MonoBehaviour
{
    public Player player;
    public Weapon curWeapon;
    private int desolve = Shader.PropertyToID("_Desolve");
    public float duration;
    [SerializeField]
    private MeshFilter containerBackFilter;
    [SerializeField]
    private MeshRenderer containerBackRenderer;
    public Transform weaponVFX;
    public Transform weaponSlot;
    public UnityAction weaponAttack;
    public bool superArmor;
    public int comboCount = 0;
    public float comboDelay = 0;
    [SerializeField]
    public float comboCoolCount = 0;
    public bool isComboCooltime;
    Coroutine weaponDesolve;

    public void WeaponSet(UnityEngine.AddressableAssets.AssetLabelReference weaponLabel)
    {
        if (curWeapon != null) AddressObject.Release(curWeapon.gameObject);
        curWeapon = AddressObject.Instinate(weaponLabel,weaponSlot).GetComponent<Weapon>();
        curWeapon.transform.GetComponent<MeshRenderer>().material.SetFloat(desolve, 1);
        containerBackRenderer.material = new Material(curWeapon.transform.GetComponent<MeshRenderer>().sharedMaterial);
        containerBackFilter.mesh = curWeapon.transform.GetComponent<MeshFilter>().sharedMesh;
        containerBackRenderer.material.SetFloat(desolve, 0);
        curWeapon.player = player;
        superArmor = curWeapon.superArmor;
        weaponAttack = curWeapon.WeaponAttack;
    }

    public IEnumerator CoMaterialNoise(bool isDisable)
    {
        float backBaseNoise = 1;
        float addNoise = 1f / duration;
        if(isDisable) {addNoise = -1*addNoise; backBaseNoise = 0;  }
        for (int i = 0; i < duration; i++)
        {
            backBaseNoise -= addNoise;
            containerBackRenderer.material.SetFloat(desolve, backBaseNoise);
            yield return null;
        }
        if (isDisable) containerBackRenderer.material.SetFloat(desolve, 1);
        else containerBackRenderer.material.SetFloat(desolve, 0);
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
            weaponVFX.localPosition = new Vector3(-0.3f, 0.5f, (float)comboCount*0.6f);
            weaponVFX.localRotation = Quaternion.Euler(new Vector3(0, 0, -15+comboCount*15));
            weaponVFX.gameObject.SetActive(true);
            weaponAttack?.Invoke();
            comboCount++;
        }
    }


 


}
