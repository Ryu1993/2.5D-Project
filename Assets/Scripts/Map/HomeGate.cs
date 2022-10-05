using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeGate : MonoBehaviour
{
    [SerializeField]
    GameObject stageSelectUI;
    private void OnCollisionEnter(Collision collision)
    {
        stageSelectUI.SetActive(true);
        Time.timeScale = 0;
    }


}
