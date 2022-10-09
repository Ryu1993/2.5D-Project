using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlinkUI : MonoBehaviour
{
    [SerializeField]
    private Image cooltime;
    [SerializeField]
    private TextMeshProUGUI count;

    public void DashCooltimeSet(Player player)
    {
        player.dashUIStart = DashCooltimeStart;
        player.dashUIcount = DashCount;
        player.dashUIcooltime = DashCooltimeCount;
        DashCount(player.maxDashable);
    }
    private void DashCooltimeStart() => cooltime.fillAmount = 1;
    private void DashCooltimeCount(float curColltimeCount)=> cooltime.fillAmount = 1 - curColltimeCount;
    private void DashCount(int curCount)=> count.text = curCount.ToString();



}
