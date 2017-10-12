using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class PropertyBool : PropertyBase<bool?>
    {

        public PropertyBool(string name, string typeName, bool value) : base(name, typeName, 0, value)
        {
        }

        public PropertyBool(string name, string typeName, int index, bool value) : base(name, typeName, index, value)
        {
        }

        public PropertyBool(ArkArchive archive, PropertyArgs args, bool propertyIsExcluded = false) : base(archive, args, propertyIsExcluded)
        {
            if (propertyIsExcluded)
            {
                archive.Position += 1;
                return;
            }

            _value = archive.GetByte() != 0;
        }

        //public override Type ValueClass => typeof(bool?);

        public override bool? Value
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
        //    return 1; // Special case: value of PropertyBool is not considered "data"
        //}

        //public override int calculateDataSize(bool nameTable)
        //{
        //    return 0;
        //}
    }
}
