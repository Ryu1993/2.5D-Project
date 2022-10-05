using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageSelectUI : MonoBehaviour
{
    List<Image> iconList = new List<Image>();

    private void Awake()
    {
        for(int i =0;i<transform.childCount;i++)
        {
            transform.GetChild(i).GetChild(0).TryGetComponent<Image>(out var icon);
            iconList.Add(icon);
        }
        transform.parent.gameObject.SetActive(false);
    }
    private IEnumerator Start()
    {
        yield return WaitList.isGameManagerSet;
        for(int i =0; i<GameManager.instance.playerInfo.curStage;i++)
        {
            iconList[i].color = Color.white;
            iconList[i].raycastTarget = true;
        }
    }

    public void ExitButtonClick()
    {
        Time.timeScale = 1f;
        transform.parent.gameObject.SetActive(false);
    }



}
