using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVFXControl : MonoBehaviour
{
    public float attackDelay;
    float count = 0;

    private void Update()
    {
        DisableCount();
    }
    void DisableCount()
    {
        count+=Time.deltaTime;
        if(count>=attackDelay)
        {
            count = 0;
            gameObject.SetActive(false);
        }
    }


}
