using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManager : RoomManager
{
    [SerializeField]
    protected GameObject bossUIPrefab;
    protected BossUI bossUI;

    protected override void BossSpawn()
    {
        monsterCounter = 1;
        BossMonster boss = AddressObject.Instinate(roomData.monster_pack).GetComponent<BossMonster>();
        boss.deadEvent += SubMonsterCountEvent;
        Instantiate(bossUIPrefab).TryGetComponent(out bossUI);
        boss.transform.position = enemyPointer.position;
        bossUI.BossConnect(boss);    
    }

}
