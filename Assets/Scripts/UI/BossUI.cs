using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUI : MonoBehaviour
{
    [SerializeField]
    private Image bossHpbar;
    [SerializeField]
    private TextMeshProUGUI bossName;
    private float maxHp;

    public void BossConnect(BossMonster boss)
    {
        bossName.text = boss.gameObject.name;
        maxHp = boss.maxHp;
        boss.bossHpEvent += BossHpBarControll;
    }
    public void BossHpBarControll(float curHp) => bossHpbar.fillAmount = curHp / maxHp;



}