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
    public abstract class StructBase : IStruct
    {
        //public abstract int getSize(bool nameTable);

        public StructBase(ArkName structType)
        {
            StructType = structType;
        }

        [JsonProperty]
        public ArkName StructType { get; set; }

        public virtual bool Native => true;

        public virtual void CollectNames(ISet<string> nameTable)
        {
            if (StructType != null) nameTable.Add(StructType.Name);
        }

    }
}
