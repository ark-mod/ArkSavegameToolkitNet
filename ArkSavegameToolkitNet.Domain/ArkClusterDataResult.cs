using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkClusterDataResult<TData> : ArkClusterDataUpdateResult
    {
        public string FilePath { get; set; }
        public TData Data { get; set; }
    }
}
