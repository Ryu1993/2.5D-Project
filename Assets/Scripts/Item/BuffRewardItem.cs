using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuffRewardItem : SceneItem,IEventable
{
    UnityAction clear;
    public void EventClear() => gameObject.SetActive(false);

    public void EventInteraction(UnityAction action)
    {
        clear = action;
        clear +=()=> Time.timeScale = 1;
    }

    protected override IEnumerator WaitAnimation()
    {
        yield return WaitList.isPlay;
        clear?.Invoke();
    }

}
