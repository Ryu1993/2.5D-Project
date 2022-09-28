using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : Singleton<LoadingUI>
{
    [SerializeField]
    GameObject loadingUI;
    [SerializeField]
    Image loadingImage;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this.gameObject);
        this.gameObject.SetActive(false);
    }
    public Coroutine CallLoading(AsyncOperation operation) => StartCoroutine(CoCallLoading(operation));

    IEnumerator CoCallLoading(AsyncOperation operation)
    {
        loadingUI.SetActive(true);
        loadingImage.fillAmount = 0f;
        float count = 0;
        while(true)
        {
            if (!operation.isDone)
            {
                count += Time.deltaTime;
                loadingImage.fillAmount = count / 10f;
            }
            else break;
            yield return null;
        }
        loadingImage.fillAmount = 1f;
        yield return WaitList.oneSecond;
        loadingUI.SetActive(false);
    }

   






}
