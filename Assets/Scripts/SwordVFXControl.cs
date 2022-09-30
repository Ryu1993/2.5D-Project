using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordVFXControl : MonoBehaviour
{
    public float duration;
    float count = 0;

    private void Update()
    {
        DisableCount();
    }
    void DisableCount()
    {
        count+=Time.deltaTime;
        if(count>=duration)
        {
            count = 0;
            gameObject.SetActive(false);
        }
    }


}
