using ArkSavegameToolkitNet.Property;
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
    public class StructPropertyList : StructBase, IPropertyContainer
    {
        public override bool Native => false;
        [JsonProperty]
        public IDictionary<ArkName, IProperty> Properties { get; set; }

        public StructPropertyList(ArkName structType) : base(structType) { }

        public StructPropertyList(ArkName structType, IDictionary<ArkName, IProperty> properties) : this(structType)
        {
            Properties = properties;
        }

        public StructPropertyList(ArkArchive archive, ArkName structType, ArkNameTree exclusivePropertyNameTree = null) : this(structType)
        {
            Properties = new Dictionary<ArkName, IProperty>();

            var property = PropertyRegistry.readProperty(archive, exclusivePropertyNameTree);
            while (property != null)
            {
                if (property != ExcludedProperty.Instance)
                    Properties.Add(ArkName.Create(property.Name.Token, property.Index), property);

                property = PropertyRegistry.readProperty(archive, exclusivePropertyNameTree);
            }
        }

        //public override int getSize(bool nameTable)
        //{
        //    int size = ArkArchive.getNameLength(qowyn.ark.properties.Property_Fields.NONE_NAME, nameTable);

        //    size += properties.Select(p => p.calculateSize(nameTable)).Sum();

        //    return size;
        //}

        //public override void CollectNames(ISet<string> nameTable)
        //{
        //    base.CollectNames(nameTable);

        //    foreach (var p in Properties.Values) p.CollectNames(nameTable);
        //}
    }
}
