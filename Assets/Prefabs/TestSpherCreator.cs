using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpherCreator : MonoBehaviour
{
    [SerializeField]
    GameObject obj;
    bool cooltime;
    WaitForSeconds wait = new WaitForSeconds(0.5f);
    float tempF = 40f;

    private void Update()
    {
        if(!cooltime)
        {
            cooltime = true;
            StartCoroutine(CreateObj());
        }
    }


    IEnumerator CreateObj()
    {

        if (Input.GetKey(KeyCode.Q)&&cooltime)
        {
            for(int i = 0; i < 9; i++)
            {
                GameObject.Instantiate(obj, transform.position, Quaternion.Euler(new Vector3(0, tempF*i, 0f)));
            }
        }
        yield return wait;
        cooltime = false;
    }


}
