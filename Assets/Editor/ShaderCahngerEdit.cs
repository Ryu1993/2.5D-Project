using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShaderChanger))]
public class ShaderCahngerEdit : Editor
{
    ShaderChanger order;

    public override void OnInspectorGUI()
    {
        order = (ShaderChanger)target;
        base.OnInspectorGUI();
        if(GUILayout.Button("ShaderChange"))
        {
            if(order.targetShader==null)
            {
                Debug.Log("ºŒ¿Ã¥ı º≥¡§");
                return;
            }
            order.SearchRenderer();
            order.ChangeShader();

        }
        
    }

}
