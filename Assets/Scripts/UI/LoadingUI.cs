using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cysharp.Threading.Tasks;

public class LoadingUI : Singleton<LoadingUI>
{
    [SerializeField]
    GameObject loadingUI;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public async UniTask CallLoading(List<UniTask> actions)
    {
        loadingUI.SetActive(true);
        float count = 0;
        float complete = actions.Count;
        foreach (var action in actions)
        {
            count++;
            Debug.Log(count / complete * 100);
            await action;
        }
        loadingUI.SetActive(false);
    }

    




}
