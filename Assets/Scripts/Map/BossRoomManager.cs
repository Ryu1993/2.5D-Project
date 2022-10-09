using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : RoomManager
{
    [SerializeField]
    protected GameObject bossUIPrefab;
    [SerializeField]
    protected GameObject endPortal;
    protected BossUI bossUI;


    protected void OpenEndGate() => endPortal.SetActive(true);

    protected override void BossSpawn()
    {
        chestReturn += OpenEndGate;
        monsterCounter = 1;
        BossMonster boss = AddressObject.Instinate(roomData.monster_pack).GetComponent<BossMonster>();
        boss.deadEvent += SubMonsterCountEvent;
        boss.deadEvent += () => boss.gameObject.SetActive(false);
        Instantiate(bossUIPrefab).TryGetComponent(out bossUI);
        boss.transform.position = enemyPointer.position;
        bossUI.BossConnect(boss);    
    }

}
