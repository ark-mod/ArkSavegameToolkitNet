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
    public class StructVector : StructBase
    {
        [JsonProperty]
        public float X { get; set; }
        [JsonProperty]
        public float Y { get; set; }
        [JsonProperty]
        public float Z { get; set; }

        public StructVector(ArkName structType) : base(structType) { }

        public StructVector(ArkName structType, float x, float y, float z) : base(structType)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public StructVector(ArkArchive archive, ArkName structType) : base(structType)
        {

            X = archive.GetFloat();
            Y = archive.GetFloat();
            Z = archive.GetFloat();
        }

        //public override int getSize(bool nameTable)
        //{
        //    return Float.BYTES * 3;
        //}
    }
}
