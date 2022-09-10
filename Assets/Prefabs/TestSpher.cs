using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpher : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 1.5f);
    }

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime;
    }

}
