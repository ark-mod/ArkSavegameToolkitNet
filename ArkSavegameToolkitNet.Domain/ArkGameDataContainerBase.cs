using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public abstract class ArkGameDataContainerBase
    {
        internal ArkGameDataBase _gameData;
        internal ArkClusterDataBase _clusterData;

        internal static readonly ArkNameTree _alldependencies = ArkNameTree.Merge(
            ArkCreature._dependencies,
            ArkTamedCreature._dependencies,
            ArkTamedCreatureAncestor._dependencies,
            ArkWildCreature._dependencies,
            ArkItem._dependencies,
            ArkPlayer._dependencies,
            ArkStructure._dependencies,
            ArkStructureCropPlot._dependencies,
            ArkStructureElectricGenerator._dependencies,
            ArkTribe._dependencies,
            ArkGameData._dependencies,
            GameObject._dependencies
            );

        internal void Initialize(ArkGameDataBase gameData, ArkClusterDataBase clusterData)
        {
            _gameData = gameData;
            _clusterData = clusterData;
        }
    }
}
