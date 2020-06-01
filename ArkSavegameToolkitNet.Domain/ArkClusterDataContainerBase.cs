using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.Domain.DataConsumers;

namespace ArkSavegameToolkitNet.Domain
{
    public abstract class ArkClusterDataContainerBase
    {
        internal ArkClusterDataBase _clusterData;

        internal static readonly ArkNameTree _alldependencies = ArkNameTree.Merge(
            ArkCloudInventory._dependencies,
            ArkCloudInventoryItem._dependencies,
            ArkCloudInventoryCharacter._dependencies,
            ArkCloudInventoryDino._dependencies,
            ArkCreature._dependencies,
            ArkTamedCreatureAncestor._dependencies,
            ArkTamedCreature._dependencies,
            //GameObject._dependencies
            DomainArkSaveConsumer._dependencies // not sure if needed
            );

        internal void Initialize(ArkClusterDataBase clusterData)
        {
            _clusterData = clusterData;
        }
    }
}
