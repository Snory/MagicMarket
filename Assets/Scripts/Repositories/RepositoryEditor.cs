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

        var repository = target as IPersistableRepository;

        if (GUILayout.Button("Add entry"))
        {
            repository.CreateEntry();
        }
        if (GUILayout.Button("Initialize repository"))
        {
            repository.Initialize();
        }
        if (GUILayout.Button("Persist repository"))
        {
            repository.Persist();
        }
    }
}