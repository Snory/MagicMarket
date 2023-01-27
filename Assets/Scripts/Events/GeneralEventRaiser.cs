using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GeneralEvent))]
public class GeneralEventRaiser : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var creatable = target as GeneralEvent;

        if (GUILayout.Button("Raise event"))
        {
            creatable.Raise();
        }
    }    
}
