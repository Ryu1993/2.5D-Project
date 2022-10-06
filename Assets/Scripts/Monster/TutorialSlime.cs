using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSlime : Monster
{

    private IEnumerator Start()
    {
        while (!isSet)
        {
            yield return null;
        }
        deadEvent += stateMachine.ResetState;
        deadEvent += TutorialManager.instance.TutorialMonsterEvent;
        deadEvent += () => this.gameObject.SetActive(false);
    }


}
