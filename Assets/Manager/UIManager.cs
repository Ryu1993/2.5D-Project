using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public HpContainerUI hpContainer;
    public WeaponUI weaponUI;
    public MapUI mapUI;


    protected override void Awake()
    {
        base.Awake();
    }
}
