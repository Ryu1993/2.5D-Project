using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    public string Name { get; }
    [Header("CardInfo")]
    public string itemCardText;
    public Sprite backBg;
    public Sprite backFrame;
    public Sprite element;
    public Sprite rarity;
    public Sprite cardIcon;
    public Sprite simpleIcon;
    public Sprite simpleRarity;
    public Sprite simpleIconFrame;
    [Header("SecenInfo")]
    public GameObject scenePrefab;
    [Header("WeaponInfo")]
    public GameObject weaponPrefab;










}
