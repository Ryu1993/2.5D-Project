using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialGate : MonoBehaviour
{
    [SerializeField]
    Collider gateColli;
    [SerializeField]
    ParticleSystem[] gateParticles;
    [SerializeField]
    StageInfo firstStage;

    public void GateOpne()
    {
        gateColli.isTrigger = true;
        foreach (var gateParticle in gateParticles) gateParticle.Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        GameManager.instance.playerInfo.curStageInfoList = null;
        GameManager.instance.playerInfo.progressStageInfo = firstStage;
        GameManager.instance.PlayerInfoSave();
        var operation = SceneManager.LoadSceneAsync("Main");
        operation.allowSceneActivation = false;
        LoadingUI.instance.CallLoading(operation);
        operation.allowSceneActivation = true;
    }


}
