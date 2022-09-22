using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Item/Artifact")]
public abstract class Artifact : Item
{
    public abstract void GetArtifactEvent(Player order);
}
