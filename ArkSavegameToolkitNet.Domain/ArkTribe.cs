using ArkSavegameToolkitNet.Arrays;
using ArkSavegameToolkitNet.Structs;
using ArkSavegameToolkitNet.Types;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkTribe : ArkGameDataContainerBase
    {
        private static readonly ArkName _tribeData = ArkName.Create("TribeData");
        private static readonly ArkName _tribeName = ArkName.Create("TribeName");
        private static readonly ArkName _tribeID = ArkName.Create("TribeID");
        private static readonly ArkName _tribeLog = ArkName.Create("TribeLog");
        private static readonly ArkName _ownerPlayerDataID = ArkName.Create("OwnerPlayerDataID");
        private static readonly ArkName _membersPlayerDataID = ArkName.Create("MembersPlayerDataID");
        private static readonly ArkName _membersPlayerName = ArkName.Create("MembersPlayerName");
        private static readonly ArkName _tribeAdmins = ArkName.Create("TribeAdmins");

        internal static readonly ArkNameTree _dependencies = new ArkNameTree
        {
            {
                _tribeData,
                new ArkNameTree
                {
                    { _tribeName, null },
                    { _tribeID, null },
                    { _tribeLog, null },
                    { _ownerPlayerDataID, null },
                    { _membersPlayerDataID, null },
                    { _membersPlayerName, null },
                    { _tribeAdmins, null }
                }
            }
        };

        internal IGameObject _tribe;

        internal void Decouple()
        {
            _tribe = null;
        }

        public ArkTribe()
        {
            // Relations
            _creatures = new Lazy<ArkTamedCreature[]>(() =>
            {
                ArkTamedCreature[] creatures = null;
                return _gameData?._tribeTamedCreatures.TryGetValue(Id, out creatures) == true ? creatures : new ArkTamedCreature[] { };
            });
            _structures = new Lazy<ArkStructure[]>(() =>
            {
                ArkStructure[] structures = null;
                return _gameData?._tribeStructures.TryGetValue(Id, out structures) == true ? structures : new ArkStructure[] { };
            });
            _items = new Lazy<ArkItem[]>(() => Structures.SelectMany(x => x.Inventory)
                .Concat(Creatures.SelectMany(x => x.Inventory)).Where(ArkItem.Filter_RealItems).ToArray());
            _creatureTypes = new Lazy<Dictionary<string, ArkTamedCreature[]>>(() => Creatures.GroupBy(x => x.ClassName).ToDictionary(x => x.Key, x => x.ToArray()));
            _structureTypes = new Lazy<Dictionary<string, ArkStructure[]>>(() => Structures.GroupBy(x => x.ClassName).ToDictionary(x => x.Key, x => x.ToArray()));
            _itemTypes = new Lazy<Dictionary<string, ArkItemTypeGroup>>(() => Items.GroupBy(x => x.ClassName).ToDictionary(x => x.Key, x => new ArkItemTypeGroup(x.ToArray())));
            _members = new Lazy<ArkPlayer[]>(() =>
            {
                ArkPlayer[] members = null;
                return _gameData?._tribePlayers.TryGetValue(Id, out members) == true ? members : new ArkPlayer[] { };
            });
            _lastActiveTime = new Lazy<DateTime>(() => Members.Length > 0 ? Members.Max(y => y.LastActiveTime) : SavedAt);
        }

        public ArkTribe(IGameObject tribe, DateTime tribeSaveTime) : this()
        {
            _tribe = tribe;

            var tribeData = tribe.GetPropertyValue<StructPropertyList>(_tribeData);
            Id = tribeData.GetPropertyValue<int>(_tribeID);
            Name = tribeData.GetPropertyValue<string>(_tribeName);
            OwnerPlayerId = tribeData.GetPropertyValue<int>(_ownerPlayerDataID);
            MemberIds = tribeData.GetPropertyValue<ArkArrayInteger>(_membersPlayerDataID)?.Where(x => x != null).Select(x => x.Value).ToArray() ?? new int[] {};
            AdminIds = tribeData.GetPropertyValue<ArkArrayInteger>(_tribeAdmins)?.Where(x => x != null).Select(x => x.Value).ToArray() ?? new int[] { };
            MemberNames = tribeData.GetPropertyValue<ArkArrayString>(_membersPlayerName)?.ToArray() ?? new string[] { };
            Logs = tribeData.GetPropertyValue<ArkArrayString>(_tribeLog)?.ToArray();

            SavedAt = tribeSaveTime;
        }

        public string Name { get; set; }
        public int Id { get; set; }
        public string[] Logs { get; set; }
        public int OwnerPlayerId { get; set; }
        public int[] MemberIds { get; set; }
        public string[] MemberNames { get; set; }
        public int[] AdminIds { get; set; }
        public DateTime SavedAt { get; set; }

        // Relations
        [JsonIgnore]
        public ArkTamedCreature[] Creatures => _creatures.Value;
        private Lazy<ArkTamedCreature[]> _creatures;

        /// <summary>
        /// Items owned by this tribe (but not by individual tribe members)
        /// </summary>
        [JsonIgnore]
        public ArkItem[] Items => _items.Value;
        private Lazy<ArkItem[]> _items;

        [JsonIgnore]
        public ArkStructure[] Structures => _structures.Value;
        private Lazy<ArkStructure[]> _structures;

        [JsonIgnore]
        public Dictionary<string, ArkTamedCreature[]> CreatureTypes => _creatureTypes.Value;
        private Lazy<Dictionary<string, ArkTamedCreature[]>> _creatureTypes;

        [JsonIgnore]
        public Dictionary<string, ArkItemTypeGroup> ItemTypes => _itemTypes.Value;
        private Lazy<Dictionary<string, ArkItemTypeGroup>> _itemTypes;

        [JsonIgnore]
        public Dictionary<string, ArkStructure[]> StructureTypes => _structureTypes.Value;
        private Lazy<Dictionary<string, ArkStructure[]>> _structureTypes;

        [JsonIgnore]
        public ArkPlayer[] Members => _members.Value;
        private Lazy<ArkPlayer[]> _members;

        [JsonIgnore]
        public DateTime LastActiveTime => _lastActiveTime.Value;
        private Lazy<DateTime> _lastActiveTime;
    }
}