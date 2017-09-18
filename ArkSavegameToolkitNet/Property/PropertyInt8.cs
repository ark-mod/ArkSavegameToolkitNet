using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class PropertyInt8 : PropertyBase<sbyte?>
    {
        public PropertyInt8(string name, string typeName, sbyte value) : base(name, typeName, 0, value)
        {
        }

        public PropertyInt8(string name, string typeName, int index, sbyte value) : base(name, typeName, index, value)
        {
        }

        public PropertyInt8(ArkArchive archive, PropertyArgs args) : base(archive, args)
        {
            _value = archive.GetByte();
        }

        public override Type ValueClass => typeof(sbyte?);

        public override sbyte? Value
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
        //    return Byte.BYTES;
        //}
    }
}
