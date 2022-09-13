using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InPoolObj : MonoBehaviour
{
    public void Return()
    {
        ObjectPoolClass.instance.Return(this);
    }
}
