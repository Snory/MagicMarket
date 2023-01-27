using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu (fileName = "NewGeneralEvent", menuName = "Events/GeneralEvent")]
public class GeneralEvent : ScriptableObject
{

    [SerializeField]
    protected List<GeneralEventListener> listeners =
        new List<GeneralEventListener>();

    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised();
    }

    public void Raise(EventArgs data)
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(data);
    }

    public void RegisterListener(GeneralEventListener listener)
    { listeners.Add(listener); }

    public void UnregisterListener(GeneralEventListener listener)
    { listeners.Remove(listener); }

}
