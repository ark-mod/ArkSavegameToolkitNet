using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class PropertyInt64 : PropertyBase<long?>
    {
        public PropertyInt64(string name, string typeName, long value) : base(name, typeName, 0, value)
        {
        }

        public PropertyInt64(string name, string typeName, int index, long value) : base(name, typeName, index, value)
        {
        }

        public PropertyInt64(ArkArchive archive, PropertyArgs args, bool propertyIsExcluded = false) : base(archive, args, propertyIsExcluded)
        {
            if (propertyIsExcluded)
            {
                archive.Position += 8;
                return;
            }

            _value = archive.GetLong();
        }

        //public override Type ValueClass => typeof(long?);

        public override long? Value
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
        //    return Long.BYTES;
        //}
    }
}
