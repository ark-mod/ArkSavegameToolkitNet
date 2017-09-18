using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkClusterDataBase : IArkClusterData
    {
        public ArkClusterDataBase(
            ArkCloudInventory[] inventories = null)
        {
            _inventories = inventories;
        }

        public ArkCloudInventory[] Inventories => _inventories ?? new ArkCloudInventory[] { };
        internal ArkCloudInventory[] _inventories;

        internal Dictionary<string, ArkCloudInventory> _playerCloudInventories;
        internal Dictionary<string, ArkCloudInventoryDino[]> _playerCloudCreatures;
        internal Dictionary<string, ArkCloudInventoryItem[]> _playerCloudItems;
        internal ArkCloudInventoryDino[] _cloudCreatures;

        internal void CopyTo(ArkClusterDataBase other)
        {
            other._inventories = _inventories;
            other._cloudCreatures = _cloudCreatures;
            other._playerCloudInventories = _playerCloudInventories;
            other._playerCloudCreatures = _playerCloudCreatures;
            other._playerCloudItems = _playerCloudItems;
        }

        internal void Initialize()
        {
            _cloudCreatures = Inventories.SelectMany(x => x.Dinos).ToArray();

            _playerCloudInventories = Inventories.ToDictionary(x => x.SteamId, x => x);

            _playerCloudCreatures = Inventories.ToDictionary(x => x.SteamId, x => x.Dinos);

            _playerCloudItems = Inventories.ToDictionary(x => x.SteamId, x => x.Items);

            //_playerCloudCreatures = Inventories.SelectMany(x => x.Dinos.Select(y => new { s = x.SteamId, d = y }))
            //    .GroupBy(x => x.s).ToDictionary(x => x.Key, x => x.Select(y => y.d).ToArray());

            //_playerCloudItems = Inventories.SelectMany(x => x.Items.Select(y => new { s = x.SteamId, i = y }))
            //    .GroupBy(x => x.s).ToDictionary(x => x.Key, x => x.Select(y => y.i).ToArray());

        }
    }
}
