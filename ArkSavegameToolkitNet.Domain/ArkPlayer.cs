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
    public enum ArkPlayerGender { Male, Female }

    public class ArkPlayer : ArkGameDataContainerBase
    {
        private static readonly ArkName _myData = ArkName.Create("MyData");
        private static readonly ArkName _myPlayerCharacterConfig = ArkName.Create("MyPlayerCharacterConfig");
        private static readonly ArkName _myPersistentCharacterStats = ArkName.Create("MyPersistentCharacterStats"); 
        private static readonly ArkName _playerDataID = ArkName.Create("PlayerDataID");
        private static readonly ArkName _uniqueID = ArkName.Create("UniqueID"); 
        private static readonly ArkName _tribeID = ArkName.Create("TribeID");
        private static readonly ArkName _tribeId = ArkName.Create("TribeId");
        private static readonly ArkName _playerName = ArkName.Create("PlayerName");
        private static readonly ArkName _savedNetworkAddress = ArkName.Create("SavedNetworkAddress");
        private static readonly ArkName _bIsFemale = ArkName.Create("bIsFemale");
        private static readonly ArkName _playerCharacterName = ArkName.Create("PlayerCharacterName");
        private static readonly ArkName _playerState_TotalEngramPoints = ArkName.Create("PlayerState_TotalEngramPoints");
        private static readonly ArkName _characterStatusComponent_ExperiencePoints = ArkName.Create("CharacterStatusComponent_ExperiencePoints");
        private static readonly ArkName _characterStatusComponent_ExtraCharacterLevel = ArkName.Create("CharacterStatusComponent_ExtraCharacterLevel");
        private static readonly ArkName[] _characterStatusComponent_NumberOfLevelUpPointsApplied = new[]
        {
            ArkName.Create("CharacterStatusComponent_NumberOfLevelUpPointsApplied", 0), //health
            ArkName.Create("CharacterStatusComponent_NumberOfLevelUpPointsApplied", 1), //stamina
            ArkName.Create("CharacterStatusComponent_NumberOfLevelUpPointsApplied", 2), //torpor
            ArkName.Create("CharacterStatusComponent_NumberOfLevelUpPointsApplied", 3), //oxygen
            ArkName.Create("CharacterStatusComponent_NumberOfLevelUpPointsApplied", 4), //food
            ArkName.Create("CharacterStatusComponent_NumberOfLevelUpPointsApplied", 5), //water
            ArkName.Create("CharacterStatusComponent_NumberOfLevelUpPointsApplied", 6), //temperature
            ArkName.Create("CharacterStatusComponent_NumberOfLevelUpPointsApplied", 7), //weight
            ArkName.Create("CharacterStatusComponent_NumberOfLevelUpPointsApplied", 8), //melee damage
            ArkName.Create("CharacterStatusComponent_NumberOfLevelUpPointsApplied", 9), //movement speed
            ArkName.Create("CharacterStatusComponent_NumberOfLevelUpPointsApplied", 10), //fortitude
            ArkName.Create("CharacterStatusComponent_NumberOfLevelUpPointsApplied", 11) //crafting speed
        };
        private static readonly ArkName _myInventoryComponent = ArkName.Create("MyInventoryComponent");

        internal static readonly ArkNameTree _dependencies = new ArkNameTree
        {
            {
                _myData,
                new ArkNameTree
                {
                    {
                        _myPlayerCharacterConfig,
                        new ArkNameTree
                        {
                            { _playerCharacterName, null },
                            { _bIsFemale, null }
                        }
                    },
                    {
                        _myPersistentCharacterStats,
                        new ArkNameTree
                        {
                            { _playerState_TotalEngramPoints, null },
                            { _characterStatusComponent_ExperiencePoints, null },
                            { _characterStatusComponent_ExtraCharacterLevel, null },
                            { _characterStatusComponent_NumberOfLevelUpPointsApplied[0], null },
                            { _characterStatusComponent_NumberOfLevelUpPointsApplied[1], null },
                            { _characterStatusComponent_NumberOfLevelUpPointsApplied[2], null },
                            { _characterStatusComponent_NumberOfLevelUpPointsApplied[3], null },
                            { _characterStatusComponent_NumberOfLevelUpPointsApplied[4], null },
                            { _characterStatusComponent_NumberOfLevelUpPointsApplied[5], null },
                            { _characterStatusComponent_NumberOfLevelUpPointsApplied[6], null },
                            { _characterStatusComponent_NumberOfLevelUpPointsApplied[7], null },
                            { _characterStatusComponent_NumberOfLevelUpPointsApplied[8], null },
                            { _characterStatusComponent_NumberOfLevelUpPointsApplied[9], null },
                            { _characterStatusComponent_NumberOfLevelUpPointsApplied[10], null },
                            { _characterStatusComponent_NumberOfLevelUpPointsApplied[11], null }
                        }
                    },
                    { _playerDataID, null },
                    { _uniqueID, null },
                    { _tribeID, null },
                    { _playerName, null },
                    { _savedNetworkAddress, null }
                }
            },
            { _myInventoryComponent, null }
        };

        internal IGameObject _profile;
        internal IGameObject _player;

        internal void Decouple()
        {
            _profile = null;
            _player = null;
        }

        public ArkPlayer()
        {
            Stats = new sbyte[_characterStatusComponent_NumberOfLevelUpPointsApplied.Length];

            // Relations
            _creatures = new Lazy<ArkTamedCreature[]>(() =>
            {
                ArkTamedCreature[] creatures = null;
                return _gameData?._playerTamedCreatures.TryGetValue(Id, out creatures) == true ? creatures : new ArkTamedCreature[] { };
            });
            _structures = new Lazy<ArkStructure[]>(() =>
            {
                ArkStructure[] structures = null;
                return _gameData?._playerStructures.TryGetValue(Id, out structures) == true ? structures : new ArkStructure[] { };
            });
            _items = new Lazy<ArkItem[]>(() => Structures.SelectMany(x => x.Inventory)
                .Concat(Creatures.SelectMany(x => x.Inventory))
                .Concat(Inventory).Where(ArkItem.Filter_RealItems).ToArray());
            _creatureTypes = new Lazy<Dictionary<string, ArkTamedCreature[]>>(() => Creatures.GroupBy(x => x.ClassName).ToDictionary(x => x.Key, x => x.ToArray()));
            _structureTypes = new Lazy<Dictionary<string, ArkStructure[]>>(() => Structures.GroupBy(x => x.ClassName).ToDictionary(x => x.Key, x => x.ToArray()));
            _itemTypes = new Lazy<Dictionary<string, ArkItemTypeGroup>>(() => Items.GroupBy(x => x.ClassName).ToDictionary(x => x.Key, x => new ArkItemTypeGroup(x.ToArray())));
            _tribe = new Lazy<ArkTribe>(() =>
            {
                ArkTribe tribe = null;
                return _gameData?._playerTribes.TryGetValue(Id, out tribe) == true ? tribe : null;
            });
            _inventory = new Lazy<ArkItem[]>(() => {
                if (!InventoryId.HasValue) return new ArkItem[] { };

                ArkItem[] items = null;
                return _gameData?._inventoryItems.TryGetValue(InventoryId.Value, out items) == true ? items.Where(ArkItem.Filter_RealItems).ToArray() : new ArkItem[] { };
            });
            _cloudCreatures = new Lazy<ArkCloudInventoryDino[]>(() =>
            {
                ArkCloudInventoryDino[] creatures = null;
                return _clusterData?._playerCloudCreatures.TryGetValue(SteamId, out creatures) == true ? creatures : new ArkCloudInventoryDino[] { };
            });
            _cloudItems = new Lazy<ArkCloudInventoryItem[]>(() =>
            {
                ArkCloudInventoryItem[] items = null;
                return _clusterData?._playerCloudItems.TryGetValue(SteamId, out items) == true ? items : new ArkCloudInventoryItem[] { };
            });
            _lastActiveTime = new Lazy<DateTime>(() =>
            {
                ArkCloudInventory inv = null;
                return _clusterData?._playerCloudInventories.TryGetValue(SteamId, out inv) == true && inv.SavedAt > SavedAt ? inv.SavedAt : SavedAt;
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="profile">.arkprofile->PrimalPlayerData</param>
        /// <param name="player">savegame->PlayerPawnTest_Female/Male_C</param>
        public ArkPlayer(IGameObject profile, IGameObject player, DateTime profileSaveTime, ISaveState saveState) : this()
        {
            _profile = profile;
            _player = player;

            var mydata = profile.GetPropertyValue<StructPropertyList>(_myData);
            var myPlayerCharacterConfig = mydata.GetPropertyValue<StructPropertyList>(_myPlayerCharacterConfig);
            var myPersistentCharacterStats = mydata.GetPropertyValue<StructPropertyList>(_myPersistentCharacterStats);
            Id = (int)mydata.GetPropertyValue<ulong>(_playerDataID);
            SteamId = mydata.GetPropertyValue<StructUniqueNetIdRepl>(_uniqueID)?.NetId;
            TribeId = mydata.GetPropertyValue<int?>(_tribeID);
            if (TribeId == null)
            {
                TribeId = mydata.GetPropertyValue<int?>(_tribeId); //genesis
            }
            Name = mydata.GetPropertyValue<string>(_playerName);
            SavedNetworkAddress = mydata.GetPropertyValue<string>(_savedNetworkAddress);
            CharacterName = myPlayerCharacterConfig.GetPropertyValue<string>(_playerCharacterName);
            Gender = myPlayerCharacterConfig.GetPropertyValue<bool?>(_bIsFemale) == true ? ArkPlayerGender.Female : ArkPlayerGender.Male;
            TotalEngramPoints = myPersistentCharacterStats.GetPropertyValue<int>(_playerState_TotalEngramPoints);
            ExperiencePoints = myPersistentCharacterStats.GetPropertyValue<float>(_characterStatusComponent_ExperiencePoints);
            CharacterLevel = (short)(myPersistentCharacterStats.GetPropertyValue<short>(_characterStatusComponent_ExtraCharacterLevel) + 1);
            for (var i = 0; i < Stats.Length; i++) Stats[i] = myPersistentCharacterStats.GetPropertyValue<sbyte?>(_characterStatusComponent_NumberOfLevelUpPointsApplied[i]) ?? 0;
            InventoryId = player?.GetPropertyValue<ObjectReference>(_myInventoryComponent)?.ObjectId;

            if (player?.Location != null) Location = new ArkLocation(player.Location, saveState);

            SavedAt = profileSaveTime;
        }

        public ArkPlayer(ArkPlayerExternal self) : this()
        {
            Id = self.Id;
            SteamId = self.SteamId;
            TribeId = self.TribeId;
            SavedAt = self.LastActiveTime;
            Name = self.Name;
            CharacterName = self.CharacterName;
            IsExternalPlayer = true;
        }

        public int Id { get; set; }
        public string SteamId { get; set; }
        public int? TribeId { get; set; }
        public string Name { get; set; }
        public string SavedNetworkAddress { get; set; }
        public string CharacterName { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ArkPlayerGender Gender { get; set; }
        public int TotalEngramPoints { get; set; }
        public float ExperiencePoints { get; set; }
        public short CharacterLevel { get; set; }
        public sbyte[] Stats { get; set; }
        public int? InventoryId { get; set; }
        public ArkLocation Location { get; set; }
        public DateTime SavedAt { get; set; }
        public bool IsExternalPlayer { get; set; }

        // Relations
        [JsonIgnore]
        public ArkTamedCreature[] Creatures => _creatures.Value;
        private Lazy<ArkTamedCreature[]> _creatures;

        /// <summary>
        /// Items owned by this player (but not their tribe)
        /// </summary>
        [JsonIgnore]
        public ArkItem[] Items => _items.Value;
        private Lazy<ArkItem[]> _items;

        [JsonIgnore]
        public ArkStructure[] Structures => _structures.Value;
        private Lazy<ArkStructure[]> _structures;

        [JsonIgnore]
        public ArkTribe Tribe => _tribe.Value;
        private Lazy<ArkTribe> _tribe;

        [JsonIgnore]
        public Dictionary<string, ArkTamedCreature[]> CreatureTypes => _creatureTypes.Value;
        private Lazy<Dictionary<string, ArkTamedCreature[]>> _creatureTypes;

        [JsonIgnore]
        public Dictionary<string, ArkItemTypeGroup> ItemTypes => _itemTypes.Value;
        private Lazy<Dictionary<string, ArkItemTypeGroup>> _itemTypes;

        [JsonIgnore]
        public Dictionary<string, ArkStructure[]> StructureTypes => _structureTypes.Value;
        private Lazy<Dictionary<string, ArkStructure[]>> _structureTypes;

        /// <summary>
        /// Items in the players inventory
        /// </summary>
        [JsonIgnore]
        public ArkItem[] Inventory => _inventory.Value;
        private Lazy<ArkItem[]> _inventory;

        [JsonIgnore]
        public ArkCloudInventoryDino[] CloudCreatures => _cloudCreatures.Value;
        private Lazy<ArkCloudInventoryDino[]> _cloudCreatures;

        [JsonIgnore]
        public ArkCloudInventoryItem[] CloudItems => _cloudItems.Value;
        private Lazy<ArkCloudInventoryItem[]> _cloudItems;

        [JsonIgnore]
        public DateTime LastActiveTime => _lastActiveTime.Value;
        private Lazy<DateTime> _lastActiveTime;
    }
}