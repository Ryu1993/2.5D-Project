using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Item/Artifact")]
public class Artifact : Item
{
    public enum ArtifactType {AttackSpeed,MaxHp,MoveSpeed,DashLength,DashCount}
    public ArtifactType artifactType;
    public float strength;

}
