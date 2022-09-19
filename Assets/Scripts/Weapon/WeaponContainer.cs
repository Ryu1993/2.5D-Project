using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponContainer : MonoBehaviour
{
    [SerializeField]
    Player player;
    public Animator animator;
    public Weapon curWeapon;
    Material material;
    int desolve = Shader.PropertyToID("_Desolve");
    [HideInInspector]
    public float duration;
    [HideInInspector]
    public bool updown;
    [SerializeField]
    MeshFilter containerBackFilter;
    [SerializeField]
    MeshRenderer containerBackRenderer;
    public UnityAction weaponAttack;
    public bool superArmor;
    public float motionTime;
    private void Start()
    {
        TestSet();
    }
    private void Update()
    {
        weaponAttack?.Invoke();
    }

    public void WeaponSet(Weapon weapon)
    {
        curWeapon = weapon;
        curWeapon.animator = animator;
        material = curWeapon.transform.GetComponent<MeshRenderer>().material;
        containerBackFilter.mesh = curWeapon.transform.GetComponent<MeshFilter>().sharedMesh;
        containerBackRenderer.material = new Material(curWeapon.transform.GetComponent<MeshRenderer>().sharedMaterial);
        containerBackRenderer.material.SetFloat(desolve, 0);
        weaponAttack += curWeapon.WeaponAttack;
        superArmor = curWeapon.superArmor;
        motionTime = curWeapon.motionTime;
        animator.SetBool(curWeapon.type.ToString(), true);
    }
    public void MaterialNoiseSet() => StartCoroutine(CoMaterialNoise());
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

    void TestSet()
    {
        if(curWeapon!=null)
        {
            WeaponSet(curWeapon);
        }

    }


}
