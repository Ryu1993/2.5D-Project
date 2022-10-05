using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    public void SceneChange(string sceneName)
    {
        loadingUI.SetActive(true);
        var operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;
        CallLoading(operation);
    }

    public Coroutine CallLoading(AsyncOperation operation)
    {
        loadingUI.SetActive(true);
        return StartCoroutine(CoCallLoading(operation));
    }

    IEnumerator CoCallLoading(AsyncOperation operation)
    { 
        loadingImage.fillAmount = 0f;
        while (!operation.isDone)
        {
            loadingImage.fillAmount = operation.progress;
            if (operation.progress > 0.8f) operation.allowSceneActivation = true;
            yield return null;
        }
        loadingImage.fillAmount = 1f;
        yield return WaitList.oneSecond;
        loadingUI.SetActive(false);
    }

  






}
