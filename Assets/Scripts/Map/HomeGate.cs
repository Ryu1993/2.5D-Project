using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeGate : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        GameManager.instance.PlayerInfoSave();
        SceneChangeMain();
    }

    private void SceneChangeMain()
    {
        var operation = SceneManager.LoadSceneAsync("Main");
        operation.allowSceneActivation = false;
        LoadingUI.instance.CallLoading(operation);
    }


}
