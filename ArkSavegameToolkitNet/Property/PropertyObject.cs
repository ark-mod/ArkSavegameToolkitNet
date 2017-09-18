using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class PropertyObject : PropertyBase<ObjectReference>
    {

        public PropertyObject(string name, string typeName, ObjectReference value) : base(name, typeName, 0, value)
        {
        }

        public PropertyObject(string name, string typeName, int index, ObjectReference value) : base(name, typeName, index, value)
        {
        }

        public PropertyObject(ArkArchive archive, PropertyArgs args) : base(archive, args)
        {
            _value = new ObjectReference(archive, DataSize);
        }

        public override Type ValueClass => typeof(ObjectReference);

        public override ObjectReference Value
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
        //    return value.getSize(nameTable);
        //}

        //protected internal override bool DataSizeNeeded
        //{
        //    get
        //    {
        //        return true;
        //    }
        //}

        public override void CollectNames(ISet<string> nameTable)
        {
            base.CollectNames(nameTable);
            _value.CollectNames(nameTable);
        }

        public override string ToString()
        {
            return "PropertyObject [value=" + Value + ", name=" + Name + ", typeName=" + TypeName + ", dataSize=" + DataSize + ", index=" + Index + "]";
        }

    }
}
