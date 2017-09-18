using ArkSavegameToolkitNet.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Structs
{
    [JsonObject(MemberSerialization.OptIn)]
    public class StructUniqueNetIdRepl : StructBase
    {
        public int Unk { get; set; }
        [JsonProperty]
        public string NetId { get; set; }

        public StructUniqueNetIdRepl(ArkName structType) : base(structType) { }

        public StructUniqueNetIdRepl(ArkName structType, int unk, string netId) : base(structType)
        {
            Unk = unk;
            NetId = netId;
        }

        public StructUniqueNetIdRepl(ArkArchive archive, ArkName structType) : base(structType)
        {
            Unk = archive.GetInt();
            NetId = archive.GetString();
        }
        
        //public override int getSize(bool nameTable)
        //{
        //    return Integer.BYTES + ArkArchive.getStringLength(netId);
        //}
    }
}
