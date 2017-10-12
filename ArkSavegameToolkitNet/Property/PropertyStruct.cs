using ArkSavegameToolkitNet.Exceptions;
using ArkSavegameToolkitNet.Structs;
using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class PropertyStruct : PropertyBase<IStruct>
    {

        public PropertyStruct(string name, string typeName, IStruct value) : base(name, typeName, 0, value)
        {
        }

        public PropertyStruct(string name, string typeName, int index, IStruct value) : base(name, typeName, index, value)
        {
        }

        public PropertyStruct(ArkArchive archive, PropertyArgs args, bool propertyIsExcluded = false, ArkNameTree exclusivePropertyNameTree = null) : base(archive, args, propertyIsExcluded)
        {
            if (propertyIsExcluded)
            {
                archive.SkipName();
                archive.Position += DataSize;
                return;
            }

            var structType = archive.GetName();
            var position = archive.Position;
            try
            {
                _value = StructRegistry.read(archive, structType, exclusivePropertyNameTree);
            }
            catch (UnreadablePropertyException)
            {
                // skip struct
                archive.Position += DataSize;
            }
        }

        //public override Type ValueClass => typeof(IStruct);

        public override IStruct Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        //protected internal override int calculateAdditionalSize(bool nameTable)
        //{
        //    return ArkArchive.getNameLength(value.StructType, nameTable);
        //}

        //public override int calculateDataSize(bool nameTable)
        //{
        //    return value.getSize(nameTable);
        //}

        //public override void CollectNames(ISet<string> nameTable)
        //{
        //    base.CollectNames(nameTable);
        //    _value.CollectNames(nameTable);
        //}
    }
}
