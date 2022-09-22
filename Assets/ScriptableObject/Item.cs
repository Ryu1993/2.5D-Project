using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class Item : ScriptableObject
{
    public enum ItemType { Equip,Artifact,Buff}
    public ItemType Type;
    public string Name { get; }
    [Header("CardInfo")]
    [TextArea]
    public string itemCardText;
    [TextArea]
    public string simpleOptionText;
    public Sprite backBg;
    public Sprite backFrame;
    public Sprite element;
    public Sprite rarity;
    public Sprite cardIcon;
    public Sprite simpleIcon;
    public Sprite simpleRarity;
    public Sprite simpleIconFrame;
    [Header("WeaponInfo")]
    public AssetLabelReference weaponPrefab;

}
