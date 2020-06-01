using ArkSavegameToolkitNet.Domain.Internal;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkAnonymizeData
    {
        public IReadOnlyDictionary<int, ExpandoObject> Players => _players;
        public IReadOnlyDictionary<int, ExpandoObject> Tribes => _tribes;
        public IReadOnlyDictionary<Tuple<uint, uint>, ExpandoObject> Tamed => _tamed;

        private Dictionary<int, ExpandoObject> _players = new Dictionary<int, ExpandoObject>();
        private Dictionary<int, ExpandoObject> _tribes = new Dictionary<int, ExpandoObject>();
        private Dictionary<Tuple<uint, uint>, ExpandoObject> _tamed = new Dictionary<Tuple<uint, uint>, ExpandoObject>();
        private int _playerNext = 0;
        private int _tribesNext = 0;
        private int _tamedNext = 0;

        public void Do(ArkPlayer i)
        {
            var anon = GetPlayer(i.Id, i);

            i.Id = anon.Id;
            i.Name = anon.Name;
            i.CharacterName = anon.CharacterName;

            if (i.TribeId.HasValue)
            {
                var tribe = GetTribe(i.TribeId.Value);
                i.TribeId = tribe.Id;
            }
            i.SteamId = anon.SteamId;
            i.SavedNetworkAddress = anon.SteamId;
        }

        public void Do(ArkTribe i)
        {
            var anon = GetTribe(i.Id);

            i.Id = anon.Id;
            i.Name = anon.Name;
            i.Logs = new string[] { };

            var owner = GetPlayer(i.OwnerPlayerId);
            i.OwnerPlayerId = (int)owner.Id;

            var members = i.MemberIds?.Select(x => GetPlayer(x)).Where(x => x != null).ToArray() ?? new dynamic[] { };
            i.MemberIds = members?.Select(x => (int)x.Id).ToArray();
            i.MemberNames = members?.Select(x => (string)x.CharacterName).ToArray();

            var admins = i.AdminIds?.Select(x => GetPlayer(x)).Where(x => x != null).ToArray() ?? new dynamic[] { };
            i.AdminIds = admins?.Select(x => (int)x.Id).ToArray();
        }

        public void Do(ArkTamedCreature i)
        {
            var anon = GetTamed(i.Id1, i.Id2, i);

            i.Id1 = anon.Id1;
            i.Id2 = anon.Id2;
            if (i.Name != null) i.Name = anon.Name;

            if ((i.OwningPlayerId.HasValue && i.OwningPlayerId.Value != i.TargetingTeam) || !i.OwningPlayerId.HasValue)
            {
                var tribe = GetTribe(i.TargetingTeam);
                i.TargetingTeam = tribe.Id;
                i.TribeName = tribe.Name;
            }
            else
            {
                i.TribeName = null;
            }

            if (i.OwningPlayerId.HasValue)
            {
                var owner = GetPlayer(i.OwningPlayerId.Value);
                i.TargetingTeam = owner.Id;
                i.OwningPlayerId = owner.Id;
                i.OwningPlayerName = owner.CharacterName;
            }

            if (i.ImprinterPlayerDataId.HasValue)
            {
                var imprinter = GetPlayer((int)i.ImprinterPlayerDataId.Value);
                i.ImprinterPlayerDataId = imprinter.Id;
                i.ImprinterName = imprinter.CharacterName;
            }

            i.TamerName = null; //can be both tribe and player
            i.TamedOnServerName = null;
        }

        public void Do(ArkStructure i)
        {
            var oldOwningPlayerId = i.OwningPlayerId;
            if (i.OwningPlayerId.HasValue)
            {
                var owner = GetPlayer(i.OwningPlayerId.Value);
                i.OwningPlayerId = owner.Id;
                i.OwningPlayerName = owner.Name;
            }

            if (i.TargetingTeam.HasValue)
            {
                if (oldOwningPlayerId.HasValue && i.TargetingTeam == oldOwningPlayerId)
                {
                    i.TargetingTeam = i.OwningPlayerId;
                    i.OwnerName = i.OwningPlayerName;
                }
                else
                {
                    var tribe = GetTribe(i.TargetingTeam.Value);
                    i.TargetingTeam = tribe.Id;
                    i.OwnerName = tribe.Name;
                }
            }
        }

        private dynamic GetPlayer(int id, ArkPlayer player = null)
        {
            var result = (dynamic)_players.GetOrCreate(id, () => {
                dynamic o = new ExpandoObject();
                var n = _playerNext++;
                o.Id = 10000000 + n;
                o.Name = GetPlayerName(id);
                o.CharacterName = GetCharacterName(id);
                o.SteamId = (10000000000000000 + n).ToString();

                return (ExpandoObject)o;
            });

            return result;
        }

        private dynamic GetTribe(int id)
        {
            var result = (dynamic)_tribes.GetOrCreate(id, () => {
                dynamic o = new ExpandoObject();
                var n = _tribesNext++;
                o.Id = 1000000000 + n;
                o.Name = GetTribeName(id);

                return (ExpandoObject)o;
            });

            return result;
        }

        private dynamic GetTamed(uint id1, uint id2, ArkTamedCreature tamed = null)
        {
            var result = (dynamic)_tamed.GetOrCreate(Tuple.Create(id1, id2), () => {
                dynamic o = new ExpandoObject();
                var n = _tamedNext++;
                o.Id1 = (uint)(1000000 + n);
                o.Id2 = (uint)(1000000 + n);
                o.Name = tamed?.Name == null ? null : GetDinoName(tamed?.ClassName, tamed?.TargetingTeam);
                //o.OldName = tamed?.Name;

                return (ExpandoObject)o;
            });

            return result;
        }

        public virtual string GetPlayerName(int id)
        {
            return "Player";
        }

        public virtual string GetCharacterName(int id)
        {
            return "Character";
        }

        public virtual string GetTribeName(int id)
        {
            return "Tribe";
        }

        public virtual string GetDinoName(string className, int? teamId)
        {
            return "Dino";
        }
    }
}
