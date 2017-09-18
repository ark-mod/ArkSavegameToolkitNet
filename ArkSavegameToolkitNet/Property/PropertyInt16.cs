using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class PropertyInt16 : PropertyBase<short?>
    {
        public PropertyInt16(string name, string typeName, short value) : base(name, typeName, 0, value)
        {
        }

        public PropertyInt16(string name, string typeName, int index, short value) : base(name, typeName, index, value)
        {
        }

        public PropertyInt16(ArkArchive archive, PropertyArgs args) : base(archive, args)
        {
            _value = archive.GetShort();
        }

        public override Type ValueClass => typeof(short?);

        public override short? Value
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
        //    return Short.BYTES;
        //}
    }
}
