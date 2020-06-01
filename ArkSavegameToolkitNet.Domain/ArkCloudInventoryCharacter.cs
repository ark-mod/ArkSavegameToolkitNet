using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.Utils.Extensions;
using System.Text.RegularExpressions;

namespace ArkSavegameToolkitNet.Domain
{
    public class ArkCloudInventoryCharacter : ArkClusterDataContainerBase
    {
        private static readonly ArkName _myArkData = ArkName.Create("MyArkData");
        private static readonly ArkName _arkPlayerData = ArkName.Create("ArkPlayerData");

        private static readonly ArkName _playerName = ArkName.Create("PlayerName");

        private Regex _r_nameLevel = new Regex(@"^(?<name>.+) - Lvl (?<level>\d+)$", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.ExplicitCapture);

        internal IPropertyContainer _character;

        internal void Decouple()
        {
            _character = null;
        }

        public ArkCloudInventoryCharacter()
        {
        }

        internal static readonly ArkNameTree _dependencies = new ArkNameTree
        {
            {
                _myArkData,
                new ArkNameTree
                {
                    {
                        _arkPlayerData,
                        new ArkNameTree
                        {
                            { _playerName, null }
                        }
                    }
                }
            }
        };

        public ArkCloudInventoryCharacter(IPropertyContainer character) : this()
        {
            _character = character;

            //todo: add more properties for cloud characters
            //todo: read the player data from byte array
            var playerName = _character.GetPropertyValue<string>(_playerName);
            if (playerName != null)
            {
                var m = _r_nameLevel.Match(playerName);
                if (m.Success)
                {
                    Name = m.Groups["name"].Value;
                    Level = int.Parse(m.Groups["level"].Value);
                }
            }
        }

        public string Name { get; set; }
        public int? Level { get; set; }
    }
}

//"MyArkData->ArkPlayerData->bForServerTransfer (Boolean)",
//"MyArkData->ArkPlayerData->bWithItems (Boolean)",
//"MyArkData->ArkPlayerData->ItemCount (Int32)",
//"MyArkData->ArkPlayerData->PlayerDataBytes (ArkArrayByte)",
//"MyArkData->ArkPlayerData->PlayerDataID (Int64)",
//"MyArkData->ArkPlayerData->PlayerName (String)",
//"MyArkData->ArkPlayerData->PlayerStats (String)",
//"MyArkData->ArkPlayerData->PlayerStats_1 (String)",
//"MyArkData->ArkPlayerData->PlayerStats_10 (String)",
//"MyArkData->ArkPlayerData->PlayerStats_11 (String)",
//"MyArkData->ArkPlayerData->PlayerStats_2 (String)",
//"MyArkData->ArkPlayerData->PlayerStats_3 (String)",
//"MyArkData->ArkPlayerData->PlayerStats_4 (String)",
//"MyArkData->ArkPlayerData->PlayerStats_5 (String)",
//"MyArkData->ArkPlayerData->PlayerStats_6 (String)",
//"MyArkData->ArkPlayerData->PlayerStats_7 (String)",
//"MyArkData->ArkPlayerData->PlayerStats_8 (String)",
//"MyArkData->ArkPlayerData->PlayerStats_9 (String)",
//"MyArkData->ArkPlayerData->UploadingServerMapName (String)",
//"MyArkData->ArkPlayerData->UploadTime (Int32)",
//"MyArkData->ArkPlayerData->Version (Single)",