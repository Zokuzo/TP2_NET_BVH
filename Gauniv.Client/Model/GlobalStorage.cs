using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gauniv.Client.Model
{
    class GlobalStorage
    {
        public static Dictionary<String, Process> ProcessIds { get; set; } = new ();
    }
}
