using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class PropertyInt32 : PropertyBase<int?>
    {
        public PropertyInt32(string name, string typeName, int value) : base(name, typeName, 0, value)
        {
        }

        public PropertyInt32(string name, string typeName, int index, int value) : base(name, typeName, index, value)
        {
        }

        public PropertyInt32(ArkArchive archive, PropertyArgs args) : base(archive, args)
        {
            _value = archive.GetInt();
        }

        public override Type ValueClass => typeof(int?);

        public override int? Value
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
        //    return Integer.BYTES;
        //}
    }
}
