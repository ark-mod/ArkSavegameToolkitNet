using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class PropertyDouble : PropertyBase<double?>
    {
        public PropertyDouble(string name, string typeName, double value) : base(name, typeName, 0, value)
        {
        }

        public PropertyDouble(string name, string typeName, int index, double value) : base(name, typeName, index, value)
        {
        }

        public PropertyDouble(ArkArchive archive, PropertyArgs args, bool propertyIsExcluded = false) : base(archive, args, propertyIsExcluded)
        {
            if (propertyIsExcluded)
            {
                archive.Position += 8;
                return;
            }


            _value = archive.GetDouble();
        }

        //public override Type ValueClass => typeof(double);

        public override double? Value
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
        //    return Double.BYTES;
        //}
    }
}
