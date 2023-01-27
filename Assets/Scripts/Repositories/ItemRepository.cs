using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class ItemRepository : Repository<Item>
{
    [ContextMenu("Persist repository")]
    public override void Persist()
    {
        base.Persist();
    }

    [ContextMenu("Initialize repostiroy")]
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void CreateEntry()
    {
        Item item = new Item();
        item.Identification = System.Guid.NewGuid().ToString();
        _entries.Add(item);
    }
}
