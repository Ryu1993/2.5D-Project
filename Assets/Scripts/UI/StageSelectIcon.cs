using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectIcon : MonoBehaviour
{
    [SerializeField]
    private StageInfo stageInfo;


    public void OnClick()
    {
        GameManager.instance.playerInfo.progressStageInfo = stageInfo;
        GameManager.instance.PlayerInfoSave();
        LoadingUI.instance.SceneChange("Main");
    }

}
