using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositoryContext : MonoBehaviour
{
    [SerializeField] private List<ScriptableObject> _persistables;

    private void Awake()
    {
        foreach(var persistable in _persistables)
        {
            Debug.Log(persistable.GetType());

            var repository = persistable as IPersistable;

            repository.Initialize();
        }
    }
}
