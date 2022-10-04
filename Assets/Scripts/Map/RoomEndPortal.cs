using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomEndPortal : MonoBehaviour
{

    private BoxCollider box;

    private void Awake()
    {
        box = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Player>(out var player))
        {
            box.enabled = false;
            AsyncOperation operation = SceneManager.LoadSceneAsync("Main");
            operation.allowSceneActivation = false;
            LoadingUI.instance.CallLoading(operation);
        }

    }

}
