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
    public class StructColor : StructBase
    {
        [JsonProperty]
        public byte B { get; set; }
        [JsonProperty]
        public byte G { get; set; }
        [JsonProperty]
        public byte R { get; set; }
        [JsonProperty]
        public byte A { get; set; }

        public StructColor(ArkName structType) : base(structType)
        {
        }

        public StructColor(ArkName structType, byte b, byte g, byte r, byte a) : base(structType)
        {
            B = b;
            G = g;
            R = r;
            A = a;
        }

        public StructColor(ArkArchive archive, ArkName structType) : base(structType)
        {
            var BGRA = archive.GetBytes(4);
            B = BGRA[0];
            G = BGRA[1];
            R = BGRA[2];
            A = BGRA[3];
        }

        //public override int getSize(bool nameTable)
        //{
        //    return Byte.BYTES * 4;
        //}
    }
}
