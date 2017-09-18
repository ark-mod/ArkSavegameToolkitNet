using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class PropertyFloat : PropertyBase<float?>
    {
        public PropertyFloat(string name, string typeName, float value) : base(name, typeName, 0, value)
        {
        }

        public PropertyFloat(string name, string typeName, int index, float value) : base(name, typeName, index, value)
        {
        }

        public PropertyFloat(ArkArchive archive, PropertyArgs args) : base(archive, args)
        {
            _value = archive.GetFloat();
        }

        public override Type ValueClass => typeof(float?);

        public override float? Value
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
        //    return Float.BYTES;
        //}
    }
}
