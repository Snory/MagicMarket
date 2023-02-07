using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMerchantDataRepository", menuName = "Repository/FileRepository/MerchantData")]
public class MerchantDataFileRepository : FileRepository<MerchantData>
{
    public override void CreateEntry()
    {
        MerchantData entry = new MerchantData();
        entry.Identification = System.Guid.NewGuid().ToString();
        _entries.Add(entry);
    }
}

