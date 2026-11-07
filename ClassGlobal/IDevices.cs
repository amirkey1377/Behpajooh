using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skydat.ClassGlobal
{
    interface IDevices
    {
        string Name { get; set; }

        List<ITechniques> SupportedTechniques { get; }

        
    }
}
