using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponUI : MonoBehaviour
{
    [SerializeField]
    Image curWeaponImage;
    [SerializeField]
    TextMeshProUGUI curWeaponName;

    public void ChangeWeapon(Equip weapon)
    {
        curWeaponImage.sprite = weapon.cardIcon;
        curWeaponImage.SetNativeSize();
        curWeaponName.text = weapon.name;
    }



}
