using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialGate : MonoBehaviour
{
    [SerializeField]
    Collider gateColli;
    [SerializeField]
    GameObject gateParticle;

    public void GateOpne()
    {
        gateColli.isTrigger = true;
        gateParticle.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        var operation = SceneManager.LoadSceneAsync("Main");
        operation.allowSceneActivation = false;
        LoadingUI.instance.CallLoading(operation);
        operation.allowSceneActivation = true;
    }


}
