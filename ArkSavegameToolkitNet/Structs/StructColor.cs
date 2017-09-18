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
        public sbyte B { get; set; }
        [JsonProperty]
        public sbyte G { get; set; }
        [JsonProperty]
        public sbyte R { get; set; }
        [JsonProperty]
        public sbyte A { get; set; }

        public StructColor(ArkName structType) : base(structType)
        {
        }

        public StructColor(ArkName structType, sbyte b, sbyte g, sbyte r, sbyte a) : base(structType)
        {
            B = b;
            G = g;
            R = r;
            A = a;
        }

        public StructColor(ArkArchive archive, ArkName structType) : base(structType)
        {

            B = archive.GetByte();
            G = archive.GetByte();
            R = archive.GetByte();
            A = archive.GetByte();
        }

        //public override int getSize(bool nameTable)
        //{
        //    return Byte.BYTES * 4;
        //}
    }
}
