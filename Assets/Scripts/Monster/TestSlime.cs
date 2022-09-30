using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSlime : Monster
{
    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(Random.Range(3, 6));
        deadEvent?.Invoke();
    }


}
