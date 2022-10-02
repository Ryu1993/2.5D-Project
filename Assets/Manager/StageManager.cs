using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search.Providers;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;

public class StageManager : Singleton<StageManager>
{
    public class MapConnetInfo
    { 
        public MapInfo curMap;
        public MapConnetInfo left = null;
        public MapConnetInfo right = null;
        public List<MapConnetInfo> prev = new List<MapConnetInfo>();
        public int stagLevel;
        public MapConnetInfo(MapInfo _curMap)
        {
            curMap = _curMap;
        }

        public bool NullNextDirection()
        {
            if (left == null && right == null)
            {
                int coin = Random.Range(0, 1);
                if (coin == 0) return true;
                else return false;
            }
            else if (left == null) return true;
            else return false;
        }
    }
    private MapConnetInfo startMap;
    private MapConnetInfo lastMap;
    private Dictionary<MapConnetInfo, StageIcon> iconsDic = new Dictionary<MapConnetInfo, StageIcon>();
    private List<MapConnetInfo> allConnetInfos = new List<MapConnetInfo>();
    private List<MapConnetInfo> disconneted = new List<MapConnetInfo>();
    private List<MapConnetInfo> conneted = new List<MapConnetInfo>();
    private int lastPrevStageLevel;
    private PlayerInfo player;
    [SerializeField]
    float stageDistance;
    [SerializeField]
    StageIcon icon;
    [SerializeField]
    Transform stagePlayer;
    NavMeshAgent playerAgent;
    [SerializeField]
    StageInfo test;

    protected override void Awake()
    {
        instance = null;
        base.Awake();
        playerAgent = stagePlayer.GetComponent<NavMeshAgent>();
        StageCreate(test);
        SceneCreate();
    }

    private IEnumerator Start()
    {
        yield return WaitList.isGameManagerSet;
        player = GameManager.instance.playerInfo;
        if(player.curStageInfoList==null)
        {
            StageCreate(player.progressStageInfo);
            player.curStageInfoList = allConnetInfos;
            player.curStageInfo = startMap;
        }
        SceneCreate();
        stagePlayer.position = iconsDic[player.curStageInfo].transform.position;
        if (player.curStageInfo.left != null) iconsDic[player.curStageInfo.left].Enable();
        if (player.curStageInfo.right != null) iconsDic[player.curStageInfo.right].Enable();
    }

    public void IconSelected(Vector3 targetPos)
    {
        iconsDic[player.curStageInfo.left].Disable();
        iconsDic[player.curStageInfo.right].Enable();
        playerAgent.SetDestination(targetPos);
        player.GetComponent<Animator>().SetBool("Move", true);
    }


    public void StageCreate(StageInfo stage)
    {
        startMap = new MapConnetInfo(null);
        startMap.stagLevel = 0;
        for (int i =0; i<stage.progressMap.Count;i++) allConnetInfos.Add(new MapConnetInfo(stage.progressMap[i]));
        foreach (MapConnetInfo mapConnetInfo in allConnetInfos) disconneted.Add(mapConnetInfo);
        conneted.Add(startMap);
        while (disconneted.Count > 0)
        {
            List<MapConnetInfo> nullNextList = NullNextSearch(ref conneted);
            MapConnetInfo curMapConnectInfo = nullNextList[Random.Range(0, nullNextList.Count)];
            MapConnetInfo targetConnectInfo = allConnetInfos[Random.Range(0, allConnetInfos.Count)];
            if (curMapConnectInfo == targetConnectInfo) continue;
            if (IsConnetable(targetConnectInfo, curMapConnectInfo.stagLevel)) continue;
            if (curMapConnectInfo.NullNextDirection()) curMapConnectInfo.left = targetConnectInfo;
            else curMapConnectInfo.right = targetConnectInfo;
            targetConnectInfo.prev.Add(curMapConnectInfo);
            if(!IsInList(targetConnectInfo,conneted))
            {
                targetConnectInfo.stagLevel = curMapConnectInfo.stagLevel + 1;
                conneted.Add(targetConnectInfo);
            }
            if (IsInList(targetConnectInfo,disconneted)) disconneted.Remove(targetConnectInfo);
        }
        lastPrevStageLevel = 0;
        foreach (var info in conneted)
        {
            if (lastPrevStageLevel < info.stagLevel) lastPrevStageLevel = info.stagLevel;
        }
        lastMap = new MapConnetInfo(stage.endMap);
        lastMap.curMap = stage.endMap;
        foreach (var info in conneted)
        {
            if (info.stagLevel == lastPrevStageLevel)
            {
                lastMap.prev.Add(info);
                info.right = lastMap;
            }
        }
        lastPrevStageLevel++;
        lastMap.stagLevel = lastPrevStageLevel;
        allConnetInfos.Add(lastMap);
        allConnetInfos.Add(startMap);
    }



    private void SceneCreate()
    {
        for(int i=0;i<lastPrevStageLevel+1;i++)
        {        
            List<MapConnetInfo> curLevelMap = new List<MapConnetInfo>();
            foreach(MapConnetInfo mapConnetInfo in allConnetInfos)
            {
                if (mapConnetInfo.stagLevel == i) curLevelMap.Add(mapConnetInfo);
            }
            Vector3 temp = new Vector3(0, 0, i * stageDistance);
            if (curLevelMap.Count % 2 == 0) temp += new Vector3(stageDistance / 2, 0, 0);
            temp -= new Vector3(stageDistance*(curLevelMap.Count/2), 0, 0);
            for(int j=0;j<curLevelMap.Count;j++)
            {
                StageIcon tempIcon = Instantiate(icon.gameObject).GetComponent<StageIcon>();
                tempIcon.transform.position = temp+new Vector3(j*stageDistance, 0, 0);
                tempIcon.CurMapConnetInfo(curLevelMap[j]);
                iconsDic.Add(curLevelMap[j], tempIcon);
            }
        }

    }
    private List<MapConnetInfo> NullNextSearch(ref List<MapConnetInfo> list)
    {
        List<MapConnetInfo> result = new List<MapConnetInfo>();
        foreach (var connectIfno in list)
        {
            if (connectIfno.right == null || connectIfno.left == null) result.Add(connectIfno);
        }
        Debug.Log(result.Count);
        return result;
    }
    private bool IsConnetable(MapConnetInfo targetInfo,int stageLevel)
    {
        if(IsInList(targetInfo,conneted))
        {
            if (targetInfo.stagLevel <= stageLevel) return true;
        }
        return false;
    }
    private bool IsInList(MapConnetInfo targetInfo,List<MapConnetInfo> list)
    {
        foreach (MapConnetInfo info in list)
        {
            if (targetInfo == info) return true;
        }
        return false;
    }




}
