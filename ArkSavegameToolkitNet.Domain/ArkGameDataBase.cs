using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkGameDataBase : IArkGameData
    {
        public ArkGameDataBase(
            SaveState saveState = null, 
            ArkTamedCreature[] tamed = null, 
            ArkWildCreature[] wild = null, 
            ArkPlayer[] players = null, 
            ArkTribe[] tribes = null, 
            ArkItem[] items = null, 
            ArkStructure[] structures = null)
        {
            SaveState = saveState;
            _tamedCreatures = tamed;
            _wildCreatures = wild;
            _players = players;
            _tribes = tribes;
            _items = items;
            _structures = structures;
        }

        public SaveState SaveState { get; internal set; }

        public ArkTamedCreature[] TamedCreatures => _tamedCreatures ?? new ArkTamedCreature[] { };
        internal ArkTamedCreature[] _tamedCreatures;

        public ArkWildCreature[] WildCreatures => _wildCreatures ?? new ArkWildCreature[] { };
        internal ArkWildCreature[] _wildCreatures;

        public ArkTribe[] Tribes =>_tribes ?? new ArkTribe[] { };
        internal ArkTribe[] _tribes;

        public ArkPlayer[] Players => _players ?? new ArkPlayer[] { };
        internal ArkPlayer[] _players;

        public ArkItem[] Items => _items ?? new ArkItem[] { };
        internal ArkItem[] _items;

        public ArkStructure[] Structures => _structures ?? new ArkStructure[] { };
        internal ArkStructure[] _structures;

        internal Dictionary<int, ArkTamedCreature[]> _playerTamedCreatures;
        internal Dictionary<int, ArkTamedCreature[]> _tribeTamedCreatures;
        internal Dictionary<int, ArkStructure[]> _playerStructures;
        internal Dictionary<int, ArkStructure[]> _tribeStructures;
        internal Dictionary<int, ArkTribe> _playerTribes;
        internal Dictionary<int, ArkPlayer[]> _tribePlayers;
        internal Dictionary<int, ArkItem[]> _inventoryItems;

        [JsonIgnore]
        public ArkTamedCreature[] Rafts { get; private set; }

        [JsonIgnore]
        public ArkTamedCreature[] NoRafts { get; private set; }

        [JsonIgnore]
        public ArkTamedCreature[] CloudCreatures { get; private set; }

        [JsonIgnore]
        public ArkTamedCreature[] InclCloud { get; private set; }

        [JsonIgnore]
        public ArkTamedCreature[] InclCloudNoRafts { get; private set; }

        //public IEnumerable<ArkTamedCreature> NoRafts => TamedCreatures?.Where(x => !x.ClassName.Equals("Raft_BP_C"));

        //public IEnumerable<ArkTamedCreature> CloudCreatures
        //{
        //    get
        //    {
        //        var cluster = _contextManager.GetCluster(Config.Cluster);
        //        var cloudCreatures = cluster?.CloudInventories?.SelectMany(x => x.Dinos);

        //        return cloudCreatures;
        //    }
        //}

        //public IEnumerable<ArkTamedCreature> InclCloud
        //{
        //    get
        //    {
        //        var cluster = _contextManager.GetCluster(Config.Cluster);
        //        var cloudCreatures = cluster?.CloudInventories?.SelectMany(x => x.Dinos);

        //        if (cloudCreatures == null) return TamedCreatures;
        //        if (TamedCreatures == null) return cloudCreatures;
        //        return cloudCreatures.Concat(TamedCreatures);
        //    }
        //}

        //public IEnumerable<ArkTamedCreature> InclCloudNoRafts
        //{
        //    get
        //    {
        //        var cluster = _contextManager.GetCluster(Config.Cluster);
        //        var cloudCreatures = cluster?.CloudInventories?.SelectMany(x => x.Dinos)?.Where(x => !x.ClassName.Equals("Raft_BP_C"));

        //        if (cloudCreatures == null) return NoRafts;
        //        if (NoRafts == null) return cloudCreatures;
        //        return cloudCreatures.Concat(NoRafts);
        //    }
        //}

        internal void CopyTo(ArkGameDataBase other)
        {
            other.SaveState = SaveState;
            other._tamedCreatures = _tamedCreatures;
            other._wildCreatures = _wildCreatures;
            other._players = _players;
            other._tribes = _tribes;
            other._items = _items;
            other._structures = _structures;
            other._playerTamedCreatures = _playerTamedCreatures;
            other._tribeTamedCreatures = _tribeTamedCreatures;
            other._playerStructures = _playerStructures;
            other._tribeStructures = _tribeStructures;
            other._playerTribes = _playerTribes;
            other._tribePlayers = _tribePlayers;
            other._inventoryItems = _inventoryItems;

            other.Rafts = Rafts;
            other.NoRafts = NoRafts;
            other.CloudCreatures = CloudCreatures;
            other.InclCloud = InclCloud;
            other.InclCloudNoRafts = InclCloudNoRafts;
        }

        internal void Initialize(ArkClusterDataBase clusterData)
        {
            _playerTamedCreatures = TamedCreatures.Where(x => x.OwningPlayerId.HasValue && x.TargetingTeam == x.OwningPlayerId.Value)
                .GroupBy(x => x.TargetingTeam).ToDictionary(x => x.Key, x => x.ToArray());
            _tribeTamedCreatures = TamedCreatures.Where(x => !x.OwningPlayerId.HasValue || x.TargetingTeam != x.OwningPlayerId.Value)
                .GroupBy(x => x.TargetingTeam).ToDictionary(x => x.Key, x => x.ToArray());

            _playerStructures = Structures.Where(x => x.OwningPlayerId.HasValue && x.TargetingTeam.HasValue && x.TargetingTeam.Value == x.OwningPlayerId.Value)
                .GroupBy(x => x.TargetingTeam.Value).ToDictionary(x => x.Key, x => x.ToArray());
            _tribeStructures = Structures.Where(x => x.TargetingTeam.HasValue && (!x.OwningPlayerId.HasValue || x.TargetingTeam.Value != x.OwningPlayerId.Value))
                .GroupBy(x => x.TargetingTeam.Value).ToDictionary(x => x.Key, x => x.ToArray());

            //a player can only have one tribe but there can be many tribes saved that have the same player (just another ark bad data issue)
            //Tribes.SelectMany(x => x.MemberIds.Select(y => new { t = x, p = y })).ToDictionary(x => x.p, x => x.t)
            _playerTribes = Players.Join(Tribes, x => x.TribeId, x => x.Id, (p, t) => new { p = p, t = t }).ToDictionary(x => x.p.Id, x => x.t);
            _tribePlayers = Players.Where(x => x.TribeId.HasValue).GroupBy(x => x.TribeId.Value).ToDictionary(x => x.Key, x => x.ToArray());

            _inventoryItems = Items.Where(x => x.OwnerInventoryId.HasValue).GroupBy(x => x.OwnerInventoryId.Value).ToDictionary(x => x.Key, x => x.ToArray());

        
            Rafts = TamedCreatures?.Where(x => x.ClassName.Equals("Raft_BP_C") || x.ClassName.Equals("MotorRaft_BP_C")).ToArray() ?? new ArkTamedCreature[] { };
            NoRafts = TamedCreatures?.Where(x => !x.ClassName.Equals("Raft_BP_C") && !x.ClassName.Equals("MotorRaft_BP_C")).ToArray() ?? new ArkTamedCreature[] { };
            CloudCreatures = clusterData?._cloudCreatures ?? new ArkTamedCreature[] { };
            InclCloud = (TamedCreatures ?? new ArkTamedCreature[] { }).Concat(CloudCreatures).ToArray();
            InclCloudNoRafts = InclCloud.Where(x => !x.ClassName.Equals("Raft_BP_C") && !x.ClassName.Equals("MotorRaft_BP_C")).ToArray();
        }
    }
}
