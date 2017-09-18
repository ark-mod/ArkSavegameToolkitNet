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
    public class StructLinearColor : StructBase
    {
        [JsonProperty]
        public float R { get; set; }
        [JsonProperty]
        public float G { get; set; }
        [JsonProperty]
        public float A { get; set; }
        [JsonProperty]
        public float B { get; set; }

        public StructLinearColor(ArkName structType) : base(structType)
        {
        }

        public StructLinearColor(ArkName structType, float r, float g, float b, float a) : base(structType)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public StructLinearColor(ArkArchive archive, ArkName structType) : base(structType)
        {

            R = archive.GetFloat();
            G = archive.GetFloat();
            B = archive.GetFloat();
            A = archive.GetFloat();
        }
        
        //public override int getSize(bool nameTable)
        //{
        //    return Float.BYTES * 4;
        //}
    }
}
