using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public abstract class Identity
{
    public string Identification;

    protected Identity()
    {
        Identification = Guid.NewGuid().ToString();
    }
}

