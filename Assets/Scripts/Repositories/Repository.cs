using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Repository<T> : MonoBehaviour, IPersistable, ICreatableEntry where T : Identity
{
    [SerializeField]
    protected List<T> _entries;

    protected virtual void Awake()
    {
        Initialize();
    }

    public virtual void AddEntry(T entry)
    {
        _entries.Add(entry);
    }

    public virtual T GetEntry(string identification)
    {
        return _entries.Where(t => t.Identification == identification).FirstOrDefault();
    }

    public abstract void CreateEntry();

    public abstract void Initialize();

    public abstract void Persist();
}
