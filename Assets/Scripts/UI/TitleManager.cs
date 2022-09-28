using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    Sprite noneActive;
    [SerializeField]
    GameObject wariningMassage;
    UnityEngine.UI.Button[] buttons = new UnityEngine.UI.Button[4];

    private void Awake()
    {
        buttons[0]= transform.Find("Play").GetComponent<UnityEngine.UI.Button>();
        buttons[1]= transform.Find("Continue").GetComponent<UnityEngine.UI.Button>();
        buttons[2]= transform.Find("Option").GetComponent<UnityEngine.UI.Button>();
        buttons[3]= transform.Find("Quit").GetComponent<UnityEngine.UI.Button>();
    }
    //private IEnumerator Start()
    //{
    //    yield return WaitList.isGameManagerSet;
    //    if (GameManager.instance.savePlayerInfo.curStage != 0) yield break;
    //    buttons[1].enabled = false;
    //    var image = buttons[1].transform.GetComponent<Image>();
    //    image.sprite = noneActive;
    //    image.raycastTarget = false;
    //}

    private void ButtonTargetDisable() { foreach (var button in buttons) button.enabled = false; }
    private void ButtonTargetEnable() { foreach (var button in buttons) button.enabled = true; }

    public void OnContinue()
    {
        ButtonTargetDisable();
        StartCoroutine(CoOnPlay("Home"));
    }
    public void OnPlay()
    {
        ButtonTargetDisable();
        if (GameManager.instance.savePlayerInfo.curStage != 0) wariningMassage.SetActive(true);
        else StartCoroutine("Tutorial");
    }

    IEnumerator CoOnPlay(string nextScene)
    {
        var operation = SceneManager.LoadSceneAsync(nextScene);
        operation.allowSceneActivation = false;
        yield return LoadingUI.instance.CallLoading(operation);
        operation.allowSceneActivation = true;
    }

    public void WaringExit()
    {
        wariningMassage.SetActive(false);
        ButtonTargetEnable();
    }
    public void WaringEnter() => StartCoroutine("Tutorial");
 


}
