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

        internal void Initialize(ArkGameDataBase gameData, ArkClusterDataBase clusterData)
        {
            _gameData = gameData;
            _clusterData = clusterData;
        }
    }
}
