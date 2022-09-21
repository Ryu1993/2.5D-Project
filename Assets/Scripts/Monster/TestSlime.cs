using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSlime : Monster
{
    private void OnEnable()
    {
        StartCoroutine(Die());

    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(Random.Range(3, 6));
        dieEvent?.Invoke();
    }


}
