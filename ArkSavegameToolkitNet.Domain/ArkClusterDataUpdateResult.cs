using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkClusterDataUpdateResult
    {
        public bool Success { get; set; }
        public bool Cancelled { get; set; }
        public TimeSpan Elapsed { get; set; }
    }
}
