using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Hit();
    public void DotHit(IAttackable.DotType type);
    public void CrowdControlHit(IAttackable.CCType type);
}
