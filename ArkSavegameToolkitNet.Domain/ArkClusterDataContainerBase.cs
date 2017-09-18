using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public abstract class ArkClusterDataContainerBase
    {
        internal ArkClusterDataBase _clusterData;

        internal void Initialize(ArkClusterDataBase clusterData)
        {
            _clusterData = clusterData;
        }
    }
}
