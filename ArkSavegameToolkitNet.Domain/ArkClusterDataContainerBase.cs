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

        internal static readonly ArkNameTree _alldependencies = ArkNameTree.Merge(
            ArkCloudInventory._dependencies,
            ArkCloudInventoryItem._dependencies,
            ArkCloudInventoryDino._dependencies,
            ArkTamedCreatureAncestor._dependencies,
            GameObject._dependencies
            );

        internal void Initialize(ArkClusterDataBase clusterData)
        {
            _clusterData = clusterData;
        }
    }
}
