using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialGate : MonoBehaviour
{
    [SerializeField]
    Collider gateColli;
    [SerializeField]
    GameObject[] gateParticles;

    public void GateOpne()
    {
        gateColli.isTrigger = true;
        foreach(var gateParticle in gateParticles) gateParticle.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        var operation = SceneManager.LoadSceneAsync("Main");
        operation.allowSceneActivation = false;
        LoadingUI.instance.CallLoading(operation);
        operation.allowSceneActivation = true;
    }


}
