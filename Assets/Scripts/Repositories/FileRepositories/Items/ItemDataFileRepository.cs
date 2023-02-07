using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemDataRepository", menuName = "Repository/FileRepository/ItemData")]
public class ItemDataFileRepository : FileRepository<ItemData>
{
    public override void CreateEntry()
    {
        ItemData entry = new ItemData();
         _entries.Add(entry);
    }
}
