using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
 
[CustomEditor(typeof(DeletePrefs))]
public class DeletePrefsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
 
        DeletePrefs script = (DeletePrefs)target;
 
        DrawDefaultInspector();
 
        if (GUILayout.Button("Delete Saves"))
        {
            script.Delete();
        }
    }
}
 