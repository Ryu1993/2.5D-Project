
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StageManager : Singleton<StageManager>
{
    public class MapConnetInfo
    { 
        public MapInfo curMap;
        [HideInInspector]
        public MapConnetInfo left = null;
        [HideInInspector]
        public MapConnetInfo right = null;
        [HideInInspector]
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
    LineRenderer line;
    [SerializeField]
    GameObject icon;
    [SerializeField]
    Transform stagePlayer;
    NavMeshAgent playerAgent;
    Animator playerAnim;

    protected override void Awake()
    {
        instance = null;
        base.Awake();
        playerAgent = stagePlayer.GetComponent<NavMeshAgent>();
        playerAnim = stagePlayer.GetComponent<Animator>();
        
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
            player.stageMaxLevel = lastPrevStageLevel;
            player.player_curHp = player.player_maxHp;
        }
        else
        {
            allConnetInfos = player.curStageInfoList;
            lastPrevStageLevel = player.stageMaxLevel;
        }
        if(player.curStageInfo.stagLevel == lastPrevStageLevel)
        {
            player.curStageInfoList = null;
            player.curStage = 1;
            GameManager.instance.PlayerInfoSave();
            LoadingUI.instance.SceneChange("Home");
            yield break;
        }
        SceneCreate();
        LineCreate();
        stagePlayer.position = iconsDic[player.curStageInfo].transform.position;
        if (player.curStageInfo.left != null) iconsDic[player.curStageInfo.left].Enable();
        if (player.curStageInfo.right != null) iconsDic[player.curStageInfo.right].Enable();
    }

    public void IconSelected(Vector3 targetPos)
    {
        if (player.curStageInfo.left != null) iconsDic[player.curStageInfo.left].Disable();
        if (player.curStageInfo.right != null) iconsDic[player.curStageInfo.right].Disable();
        playerAgent.SetDestination(targetPos);
        playerAnim.SetBool("Move", true);
    }
    public void StageCreate(StageInfo stage)
    {
        startMap = new MapConnetInfo(null);
        startMap.stagLevel = 0;
        for (int i =0; i<stage.progressMap.Count;i++)
        {
            allConnetInfos.Add(new MapConnetInfo(stage.progressMap[i]));
        }
        foreach (MapConnetInfo mapConnetInfo in allConnetInfos)
        {
            disconneted.Add(mapConnetInfo);
        }
        conneted.Add(startMap);
        while (disconneted.Count > 0)
        {
            List<MapConnetInfo> nullNextList = NullNextSearch(ref conneted);
            MapConnetInfo curMapConnectInfo = nullNextList[Random.Range(0, nullNextList.Count)];
            MapConnetInfo targetConnectInfo = allConnetInfos[Random.Range(0, allConnetInfos.Count)];
            if (curMapConnectInfo == targetConnectInfo) continue;
            if (IsDisConnetable(targetConnectInfo, curMapConnectInfo.stagLevel+1)) continue;
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
        foreach (MapConnetInfo info in allConnetInfos)
        {
            if (info == lastMap) return;
            if(info.left==null&&info.right==null)
            {
                int coin = Random.Range(0, 2);
                if (coin == 1) info.left = NextLevelInfo(info.stagLevel);
                else info.right = NextLevelInfo(info.stagLevel);
            }
        }
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
                StageIcon tempIcon = Instantiate(icon).GetComponent<StageIcon>();
                tempIcon.transform.position = temp+new Vector3(j*stageDistance, 0, 0);
                tempIcon.CurMapConnetInfo(curLevelMap[j]);
                iconsDic.Add(curLevelMap[j], tempIcon);
            }
        }
    }

    
    private MapConnetInfo NextLevelInfo(int stageLevel)
    {
        List<MapConnetInfo> nextLevels = new List<MapConnetInfo>();
        foreach(var info in allConnetInfos)
        {
            if(info.stagLevel == stageLevel+1)
            {
                nextLevels.Add(info);
            }
        }
        return nextLevels[Random.Range(0,nextLevels.Count)];
    }



    private void LineCreate()
    {
        foreach(MapConnetInfo info in allConnetInfos)
        {
            iconsDic[info].TryGetComponent(out Transform cur);
            if(info.left!=null)
            {
                iconsDic[info.left].TryGetComponent(out Transform target);
                LineRenderer templine = Instantiate(line);
                templine.SetPositions(new Vector3[2] {cur.position,target.position });
            }
            if(info.right!=null)
            {
                iconsDic[info.right].TryGetComponent(out Transform target);
                LineRenderer templine = Instantiate(line);
                templine.SetPositions(new Vector3[2] { cur.position, target.position });
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
        return result;
    }
    private bool IsDisConnetable(MapConnetInfo targetInfo,int stageLevel)
    {
        if(IsInList(targetInfo,conneted))
        {
            if (targetInfo.stagLevel != stageLevel) return true;
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
