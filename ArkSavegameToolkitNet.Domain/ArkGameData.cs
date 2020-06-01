using ArkSavegameToolkitNet.DataConsumers;
using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;
using ArkSavegameToolkitNet.Domain.DataConsumers;
using ArkSavegameToolkitNet.Domain.Utils.Extensions;
using ArkSavegameToolkitNet.Utils;
using ArkSavegameToolkitNet.Utils.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkGameData : ArkGameDataBase
    {
        private readonly string _saveFilePath;
        private readonly ArkClusterDataBase _clusterData;
        private readonly bool _loadProfiles;
        private readonly bool _loadTribes;
        private readonly bool _loadOnlyPropertiesInDomain;
        private dynamic[] _newData;

        private static readonly ArkName _myCharacterStatusComponent = ArkName.Create("MyCharacterStatusComponent");
        private static readonly ArkName _myData = ArkName.Create("MyData");
        private static readonly ArkName _playerDataID = ArkName.Create("PlayerDataID");
        private static readonly ArkName _linkedPlayerDataID = ArkName.Create("LinkedPlayerDataID");
        private static readonly ArkName _uniqueID = ArkName.Create("UniqueID");

        internal static readonly ArkNameTree _dependencies = new ArkNameTree
        {
            { _myCharacterStatusComponent, null },
            {
                _myData,
                new ArkNameTree
                {
                    { _playerDataID, null },
                    { _uniqueID, null }
                }
            },
            { _linkedPlayerDataID, null }
        };

        public ArkGameData(string saveFilePath, string clusterSavePath = null, bool loadProfiles = true, bool loadTribes = true, bool loadOnlyPropertiesInDomain = false)
        {
            _saveFilePath = saveFilePath;
            _clusterData = !string.IsNullOrEmpty(clusterSavePath) ? new ArkClusterData(clusterSavePath) : null;
            _loadProfiles = loadProfiles;
            _loadTribes = loadTribes;
            _loadOnlyPropertiesInDomain = loadOnlyPropertiesInDomain;
        }

        public ArkGameData(string saveFilePath, ArkClusterDataBase clusterData, bool loadProfiles = true, bool loadTribes = true, bool loadOnlyPropertiesInDomain = false)
        {
            _saveFilePath = saveFilePath;
            _clusterData = clusterData;
            _loadProfiles = loadProfiles;
            _loadTribes = loadTribes;
            _loadOnlyPropertiesInDomain = loadOnlyPropertiesInDomain;
        }

        /// <param name="externalPlayerData">Supply the update operation with external player data. This is a way to make sure transfered players are not lost inbetween saves.</param>
        public ArkGameDataUpdateResult Update(CancellationToken ct, ArkPlayerExternal[] externalPlayerData = null, bool deferApplyNewData = false, ArkAnonymizeData anonymize = null)
        {
            var success = false;
            var cancelled = false;
            var st = Stopwatch.StartNew();

            try
            {
                var directoryPath = Path.GetDirectoryName(_saveFilePath);
                var exclusivePropertyNameTree = _loadOnlyPropertiesInDomain ? ArkGameDataContainerBase._alldependencies : null;

                // Extract all game data
                var consumer = new DomainArkSaveConsumer();

                ArkToolkit.DefaultSettings = () =>
                {
                    var settings = new ArkToolkitLoaderSettings();
                    settings.EnableStructureLog = false;
                    settings.BufferSize = 4096 * 100;
                    settings.DataConsumerProvider = () => new NullDataConsumer();
                    settings.DevFlags = DevFlags.None;
                    settings.ExclusivePropertyNameTree = exclusivePropertyNameTree;
                    return settings;
                };

                //save = ArkToolkit.LoadArkSave(_saveFilePath);
                var settings = ArkToolkit.DefaultSettings.Invoke();
                settings.DataConsumerProvider = () => consumer;
                ArkToolkitLoader.Create(settings).LoadArkSave(_saveFilePath);

                var save = consumer.SaveData;

                ct.ThrowIfCancellationRequested();

                var arktribes = _loadTribes ? Directory.GetFiles(directoryPath, "*.arktribe", SearchOption.TopDirectoryOnly).Select(x =>
                {
                    try
                    {
                        return ArkToolkit.LoadTribeSave(x);
                    }
                    catch (Exception ex) { Debug.WriteLine($"Failed to extract tribe profile '{x}': {ex.Message}"); }

                    return null;
                    
                }).Where(x => x != null).ToArray() : new TribeSaveGameData[] { };
                ct.ThrowIfCancellationRequested();

                var arkprofiles = _loadProfiles ? Directory.GetFiles(directoryPath, "*.arkprofile", SearchOption.TopDirectoryOnly).Select(x =>
                {
                    try
                    {
                        return ArkToolkit.LoadProfileSave(x);
                    }
                    catch(Exception ex) { Debug.WriteLine($"Failed to extract player profile '{x}': {ex.Message}"); }

                    return null;
                }).Where(x => x != null).ToArray() : new ProfileSaveGameData[] { };
                ct.ThrowIfCancellationRequested();

                // Map all game data into domain model
                var tamed = consumer.TamedCreatures;
                var wild = consumer.WildCreatures;

                var _myData = ArkName.Create("MyData");
                var _playerDataID = ArkName.Create("PlayerDataID");
                var _linkedPlayerDataID = ArkName.Create("LinkedPlayerDataID");
                var _uniqueID = ArkName.Create("UniqueID");

                var keyedProfiles = arkprofiles.Select(x =>
                {
                    var primalPlayerData = x.objects.FirstOrDefault(x => x.className.Token.Equals("PrimalPlayerData") || x.className.Token.Equals("PrimalPlayerDataBP_C"));
                    if (primalPlayerData == null) return null;

                    var mydata = primalPlayerData.GetProperty<PropertyListStructProperty>(_myData);
                    return new
                    {
                        steamId = mydata.GetProperty<UniqueNetIdReplStructProperty>(_uniqueID)?.netId,
                        playerId = mydata.GetPropertyValue<ulong>(_playerDataID),
                        profile = x
                    };
                }).Where(x => x != null).ToArray();
                var profileSteamIds = keyedProfiles.Select(x => x.steamId).Where(x => x != null).Distinct();

                var players = keyedProfiles.Select(x =>
                {
                    var player = consumer.PlayerCharactersMap[x.playerId]?.FirstOrDefault();
                    try
                    {
                        var primalPlayerData = x.profile.objects.FirstOrDefault(x => x.className.Token.Equals("PrimalPlayerData") || x.className.Token.Equals("PrimalPlayerDataBP_C"));
                        if (primalPlayerData == null) return null;

                        return primalPlayerData.AsPlayer(player, x.profile.savedAt, save);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Failed to project player profile as domain object '{x.profile.fileName}': {ex.Message}");
                    }

                    return null;
                }).Where(x => x != null).ToArray();
                var externalPlayers = externalPlayerData != null ?
                    externalPlayerData.Where(x => !profileSteamIds.Contains(x.SteamId, StringComparer.OrdinalIgnoreCase)).Select(x => x.AsPlayer()).ToArray() 
                    : new ArkPlayer[] { };

                var allplayers = players.Concat(externalPlayers).ToArray();

                var tribes = arktribes.Select(x =>
                {
                    var primalTribeData = x.objects.FirstOrDefault(x => x.className.Token.Equals("PrimalTribeData"));
                    if (primalTribeData == null) return null;

                    return primalTribeData.AsTribe(x.savedAt);
                }).Where(x => x != null).ToArray();

                var items = consumer.Items;
                var structures = consumer.Structures;

                ApplyOrSaveNewData(deferApplyNewData, save, tamed.ToArray(), wild.ToArray(), allplayers, tribes, items.ToArray(), structures.ToArray(), anonymize);

                success = true;
            }
            catch (OperationCanceledException)
            {
                cancelled = true;
            }

            return new ArkGameDataUpdateResult { Success = success, Cancelled = cancelled, Elapsed = st.Elapsed };
        }

        public bool ApplyPreviousUpdate(bool decouple = true)
        {
            if (_newData != null)
            {
                ApplyNewData(_newData[0], _newData[1], _newData[2], _newData[3], _newData[4], _newData[5], _newData[6], decouple, _newData[7]);
                _newData = null;
                return true;
            }
            else return false;
        }

        private void ApplyOrSaveNewData(bool deferApplyNewData, ArkSaveData save, ArkTamedCreature[] tamed, ArkWildCreature[] wild, ArkPlayer[] players, ArkTribe[] tribes, ArkItem[] items, ArkStructure[] structures, ArkAnonymizeData anonymize = null)
        {
            if (deferApplyNewData)
            {
                _newData = new dynamic[] { save, tamed, wild, players, tribes, items, structures, anonymize };
            }
            else
            {
                ApplyNewData(save, tamed, wild, players, tribes, items, structures, true, anonymize);
            }
        }

        private void ApplyNewData(ArkSaveData save, ArkTamedCreature[] tamed, ArkWildCreature[] wild, ArkPlayer[] players, ArkTribe[] tribes, ArkItem[] items, ArkStructure[] structures, bool decouple = true, ArkAnonymizeData anonymize = null)
        {
            // Anonymize data if requested
            if (anonymize != null)
            {
                foreach (var i in players) anonymize.Do(i);
                foreach (var i in tribes) anonymize.Do(i);
                foreach (var i in tamed) anonymize.Do(i);
                foreach (var i in structures) anonymize.Do(i);
            }

            // Setup relations in the domain model between entities
            var newGameData = new ArkGameDataBase(SaveState.FromArkSaveData(save), tamed, wild, players, tribes, items, structures);
            newGameData.Initialize(_clusterData);
            foreach (var i in tamed) i.Initialize(newGameData, _clusterData);
            foreach (var i in wild) i.Initialize(newGameData, _clusterData);
            foreach (var i in players) i.Initialize(newGameData, _clusterData);
            foreach (var i in tribes) i.Initialize(newGameData, _clusterData);
            foreach (var i in items) i.Initialize(newGameData, _clusterData);
            foreach (var i in structures) i.Initialize(newGameData, _clusterData);

            if (decouple) //should always be true except for debugging
            {
                // Unset all references to raw extracted game objects and properties to free memory
                foreach (var i in tamed) i.Decouple();
                foreach (var i in wild) i.Decouple();
                foreach (var i in players) i.Decouple();
                foreach (var i in tribes) i.Decouple();
                foreach (var i in items) i.Decouple();
                foreach (var i in structures) i.Decouple();
            }

            // Assign updated data to public properties
            newGameData.CopyTo(this);

            // Force an immediate garbage collection because it seems more effective (extraction process requires a great deal of memory)
            //GC.Collect();
        }
    }
}
