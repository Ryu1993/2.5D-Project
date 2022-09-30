using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public HpContainerUI hpContainer;
    public WeaponUI weaponUI;
    public MapUI mapUI;
    public ArtifactUI artifactUI;
    public RewardCardWindow rewardCardWindow;

    protected override void Awake()
    {
        instance = null;
        base.Awake();
    }
}
