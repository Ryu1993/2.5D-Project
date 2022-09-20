using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardCardSet : MonoBehaviour
{
    public Item item;
    [SerializeField]
    Image backBg;
    [SerializeField]
    Image backFrame;
    [SerializeField]
    Image element;
    [SerializeField]
    Image rarity;
    [SerializeField]
    Image cardIcon;
    [SerializeField]
    Image simpleIcon;
    [SerializeField]
    Image simpleRarity;
    [SerializeField]
    Image simpleIconFrame;
    [SerializeField]
    TextMeshProUGUI nameTag;
    [SerializeField]
    TextMeshProUGUI itemText;


    public void OnEnable()
    {
        backBg.sprite = item.backBg;
        backFrame.sprite = item.backFrame;
        element.sprite = item.element;
        rarity.sprite = item.rarity;
        cardIcon.sprite = item.cardIcon;
        simpleIcon.sprite = item.simpleIcon;
        simpleRarity.sprite = item.simpleRarity;
        nameTag.text = item.Name;
        itemText.text = item.itemCardText;
    }






}
