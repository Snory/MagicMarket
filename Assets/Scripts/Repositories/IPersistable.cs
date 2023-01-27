using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public interface IPersistable
{
    public void Persist();
    public void Initialize();
}
