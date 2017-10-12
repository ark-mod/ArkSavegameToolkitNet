using ArkSavegameToolkitNet.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class PropertyName : PropertyBase<ArkName>
    {
        public PropertyName(string name, string typeName, ArkName value) : base(name, typeName, 0, value)
        {
        }

        public PropertyName(string name, string typeName, int index, ArkName value) : base(name, typeName, index, value)
        {
        }

        public PropertyName(ArkArchive archive, PropertyArgs args, bool propertyIsExcluded = false) : base(archive, args, propertyIsExcluded)
        {
            if (propertyIsExcluded)
            {
                archive.SkipName();
                return;
            }

            _value = archive.GetName();
        }

        //public override Type ValueClass => typeof(ArkName);

        public override ArkName Value
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
        //    return ArkArchive.getNameLength(value, nameTable);
        //}

        //public override void CollectNames(ISet<string> nameTable)
        //{
        //    base.CollectNames(nameTable);
        //    nameTable.Add(_value.Name);
        //}
    }
}
