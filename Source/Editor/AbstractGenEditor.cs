using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractGen), true)]
public class AbstractGenEditor : Editor
{
    private AbstractGen gen;

    private void Awake(){
        gen = (AbstractGen)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate Dungeon")){
            gen.Generate();
        }
    }
}
