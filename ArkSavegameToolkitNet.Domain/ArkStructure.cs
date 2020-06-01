using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;
using ArkSavegameToolkitNet.Utils.Extensions;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkStructure : ArkGameDataContainerBase
    {
        private static readonly ArkName _targetingTeam = ArkName.Create("TargetingTeam");
        private static readonly ArkName _ownerName = ArkName.Create("OwnerName");
        private static readonly ArkName _owningPlayerID = ArkName.Create("OwningPlayerID");
        private static readonly ArkName _owningPlayerName = ArkName.Create("OwningPlayerName");
        private static readonly ArkName _myInventoryComponent = ArkName.Create("MyInventoryComponent");
        private static readonly ArkName _boxName = ArkName.Create("BoxName");
        private static readonly ArkName _attachedToDinoId1 = ArkName.Create("AttachedToDinoID1");

        internal static readonly ArkNameTree _dependencies = new ArkNameTree
        {
            { _ownerName, null },
            { _targetingTeam, null },
            { _owningPlayerID, null },
            { _owningPlayerName, null },
            { _myInventoryComponent, null },
            { _boxName, null },
            { _attachedToDinoId1, null }
        };

        internal GameObject _structure;

        internal void Decouple()
        {
            _structure = null;
        }

        public ArkStructure()
        {
            // Relations
            _inventory = new Lazy<ArkItem[]>(() => {
                if (!InventoryId.HasValue) return new ArkItem[] { };

                ArkItem[] items = null;
                return _gameData?._inventoryItems.TryGetValue(InventoryId.Value, out items) == true ? items.Where(ArkItem.Filter_RealItems).ToArray() : new ArkItem[] { };
            });
        }

        public ArkStructure(GameObject structure, ArkSaveData saveState) : this()
        {
            _structure = structure;

            ClassName = structure.className.Name;
            //Id = structure.Index;
            //Uuid = _structure.Uuid;
            OwnerName = structure.GetPropertyValue<string>(_ownerName);
            TargetingTeam = structure.GetPropertyValue<int?>(_targetingTeam);
            OwningPlayerId = structure.GetPropertyValue<int?>(_owningPlayerID);
            OwningPlayerName = structure.GetPropertyValue<string>(_owningPlayerName);
            InventoryId = structure.GetProperty<ObjectReferenceIdProperty>(_myInventoryComponent)?.id;
            BoxName = structure.GetPropertyValue<string>(_boxName);
            AttachedToDinoId1 = structure.GetPropertyValue<uint?>(_attachedToDinoId1);

            if (structure?.location != null) Location = new ArkLocation(structure.location.Value, saveState);
        }

        //public int Id { get; set; }
        //public Guid Uuid { get; set; } //not unique?
        public string ClassName { get; set; }
        public string OwnerName { get; set; }
        public int? TargetingTeam { get; set; }
        public int? OwningPlayerId { get; set; }
        public string OwningPlayerName { get; set; }
        public int? InventoryId { get; set; }
        public string BoxName { get; set; }
        public uint? AttachedToDinoId1 { get; set; }
        public ArkLocation Location { get; set; }

        // Relations
        [JsonIgnore]
        public ArkItem[] Inventory => _inventory.Value;
        private Lazy<ArkItem[]> _inventory;
    }
}