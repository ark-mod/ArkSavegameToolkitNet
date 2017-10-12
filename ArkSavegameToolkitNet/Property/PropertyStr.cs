using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class PropertyStr : PropertyBase<string>
    {
        public PropertyStr(string name, string typeName, string value) : base(name, typeName, 0, value)
        {
        }

        public PropertyStr(string name, string typeName, int index, string value) : base(name, typeName, index, value)
        {
        }

        public PropertyStr(ArkArchive archive, PropertyArgs args, bool propertyIsExcluded = false) : base(archive, args, propertyIsExcluded)
        {
            if (propertyIsExcluded)
            {
                archive.SkipString();
                return;
            }

            _value = archive.GetString();
        }

        //public override Type ValueClass => typeof(string);

        public override string Value
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

        //public override int calculateDataSize(bool nameTable)
        //{
        //    return ArkArchive.getStringLength(value);
        //}
    }
}
