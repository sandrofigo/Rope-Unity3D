using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Rope))]
public class RopeEditor : Editor {

    Rope rope;

    public override void OnInspectorGUI()
    {
        rope = (Rope)target;

        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        
    }
}
