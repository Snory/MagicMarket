using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(Repository<>),true)]
public class RepositoryEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("Common repository");
        var creatable = target as ICreatableEntry;

        if (GUILayout.Button("Add entry"))
        {
            creatable.CreateEntry();
        }
        
        var persistable = target as IPersistable;
        if (GUILayout.Button("Initialize repository"))
        {
            persistable.Initialize();
        }
        if (GUILayout.Button("Persist repository"))
        {
            persistable.Persist();
        }
    }
}