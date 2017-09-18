using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class PropertyText : PropertyBase<string>
    {
        public PropertyText(string name, string typeName, string value) : base(name, typeName, 0, value)
        {
        }

        public PropertyText(string name, string typeName, int index, string value) : base(name, typeName, index, value)
        {
        }

        public PropertyText(ArkArchive archive, PropertyArgs args) : base(archive, args)
        {
            _value = System.Convert.ToBase64String((byte[])(Array)archive.GetBytes(DataSize));
        }

        public override Type ValueClass => typeof(string);

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
        //    return DECODER.decode(value).length;
        //}
    }
}
