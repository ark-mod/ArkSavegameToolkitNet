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
    public class StructQuat : StructBase
    {
        [JsonProperty]
        public float X { get; set; }
        [JsonProperty]
        public float Y { get; set; }
        [JsonProperty]
        public float Z { get; set; }
        [JsonProperty]
        public float W { get; set; }

        public StructQuat(ArkName structType) : base(structType) { }

        public StructQuat(ArkName structType, float x, float y, float z, float w) : base(structType)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public StructQuat(ArkArchive archive, ArkName structType) : base(structType)
        {

            X = archive.GetFloat();
            Y = archive.GetFloat();
            Z = archive.GetFloat();
            W = archive.GetFloat();
        }

        //public override int getSize(bool nameTable)
        //{
        //    return Float.BYTES * 4;
        //}
    }
}
