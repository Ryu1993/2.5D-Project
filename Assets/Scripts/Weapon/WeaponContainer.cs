using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponContainer : MonoBehaviour
{
    public Player player;
    public Animator animator;
    public Weapon curWeapon;
    private Material material;
    private int desolve = Shader.PropertyToID("_Desolve");
    [HideInInspector]
    public float duration;
    [HideInInspector]
    public bool updown;
    [SerializeField]
    private MeshFilter containerBackFilter;
    [SerializeField]
    private MeshRenderer containerBackRenderer;
    public Transform weaponSlot;
    public UnityAction weaponAttack;
    public bool superArmor;
    public bool isProgress;
    public bool isoffDelay;
    public int bufferedInput;

    public void WeaponSet(Weapon weapon)
    {
        curWeapon = weapon;
        curWeapon.animator = animator;
        curWeapon.player = player;
        material = curWeapon.transform.GetComponent<MeshRenderer>().material;
        containerBackFilter.mesh = curWeapon.transform.GetComponent<MeshFilter>().sharedMesh;
        containerBackRenderer.material = new Material(curWeapon.transform.GetComponent<MeshRenderer>().sharedMaterial);
        containerBackRenderer.material.SetFloat(desolve, 0);
        superArmor = curWeapon.superArmor;
        weaponAttack = curWeapon.WeaponAttack;
        animator.SetBool(curWeapon.type.ToString(), true);
    }


    private void CurWeaponAttack()
    {
        weaponAttack?.Invoke();
    }
    private void MaterialNoiseSet() => StartCoroutine(CoMaterialNoise());
    private IEnumerator CoMaterialNoise()
    {
        float baseNoise = 0;
        float backBaseNoise = 1;
        float addNoise = 1f / duration;
        if(updown) { baseNoise = 1; addNoise = -1*addNoise; backBaseNoise = 0;  }
        for (int i = 0; i < duration; i++)
        {
            baseNoise += addNoise;
            backBaseNoise -= addNoise;
            material.SetFloat(desolve, baseNoise);
            containerBackRenderer.material.SetFloat(desolve, backBaseNoise);
            yield return null;
        }
        if (updown)
        {
            material.SetFloat(desolve, 0);
            containerBackRenderer.material.SetFloat(desolve, 1);
        }
        else
        {
            material.SetFloat(desolve, 1); 
            containerBackRenderer.material.SetFloat(desolve, 0);
        }
    }

    public void WeaponContainerReady()
    {
        material?.SetFloat(desolve, 1);
        containerBackRenderer?.material.SetFloat(desolve, 0);
    }
    public void WeaponAnimationOn() => animator.SetBool("Action", true);
    public void WeaponAnimationOff()
    {
        if (!isoffDelay)
        {
            animator.SetBool("Action", false);
            Coroutine coroutine = StartCoroutine(CoDelay());
        }
    }
    protected IEnumerator CoDelay()
    {
        isoffDelay = true;
        for (int i = 0; i < bufferedInput; i++)
        {
            yield return null;
        }
        isoffDelay = false;
    }

    private void Start()
    {
        TestSet();
    }

    void TestSet()
    {
        if(curWeapon!=null)
        {
            WeaponSet(curWeapon);
        }

    }


}
