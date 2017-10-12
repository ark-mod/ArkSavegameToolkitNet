using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class PropertyByte : PropertyBase<ArkByteValue>
    {

        public PropertyByte(string name, string typeName, ArkByteValue value) : base(name, typeName, 0, value)
        {
        }

        public PropertyByte(string name, string typeName, int index, ArkByteValue value) : base(name, typeName, index, value)
        {
        }

        public PropertyByte(ArkArchive archive, PropertyArgs args, bool propertyIsExcluded = false) : base(archive, args, propertyIsExcluded)
        {
            var enumName = archive.GetName();
            _value = new ArkByteValue(archive, enumName, propertyIsExcluded);
        }

        //public override Type ValueClass => typeof(ArkByteValue);

        public override ArkByteValue Value
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
        //    return ArkArchive.getNameLength(value.EnumName, nameTable);
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
