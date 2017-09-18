using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Data
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ExtraDataBlob : IExtraData
    {
        [JsonProperty]
        public sbyte[] Data { get; set; }

        //public int calculateSize(bool nameTable)
        //{
        //    return data != null ? data.Length : 0;
        //}
    }
}
