using System.Collections.Generic;
using System.Linq;

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
        internal Dictionary<string, ArkCloudInventoryCharacter[]> _playerCloudCharacters;
        internal Dictionary<string, ArkCloudInventoryItem[]> _playerCloudItems;
        internal ArkCloudInventoryDino[] _cloudCreatures;

        internal void CopyTo(ArkClusterDataBase other)
        {
            other._inventories = _inventories;
            other._cloudCreatures = _cloudCreatures;
            other._playerCloudInventories = _playerCloudInventories;
            other._playerCloudCreatures = _playerCloudCreatures;
            other._playerCloudCharacters = _playerCloudCharacters;
            other._playerCloudItems = _playerCloudItems;
        }

        internal void Initialize()
        {
            _cloudCreatures = Inventories.SelectMany(x => x.Dinos).ToArray();

            _playerCloudInventories = Inventories.ToDictionary(x => x.SteamId, x => x);

            _playerCloudCreatures = Inventories.ToDictionary(x => x.SteamId, x => x.Dinos);

            _playerCloudCharacters = Inventories.ToDictionary(x => x.SteamId, x => x.Characters);

            _playerCloudItems = Inventories.ToDictionary(x => x.SteamId, x => x.Items);
        }
    }
}
