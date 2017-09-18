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
    public class StructVector2d : StructBase
    {
        [JsonProperty]
        public float X { get; set; }
        [JsonProperty]
        public float Y { get; set; }

        public StructVector2d(ArkName structType) : base(structType) { }

        public StructVector2d(ArkName structType, float x, float y) : base(structType)
        {
            X = x;
            Y = y;
        }

        public StructVector2d(ArkArchive archive, ArkName structType) : base(structType)
        {
            X = archive.GetFloat();
            Y = archive.GetFloat();
        }

        //public override int getSize(bool nameTable)
        //{
        //    return Float.BYTES * 2;
        //}
    }
}
