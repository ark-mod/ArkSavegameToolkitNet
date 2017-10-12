using ArkSavegameToolkitNet.Arrays;
using ArkSavegameToolkitNet.Exceptions;
using ArkSavegameToolkitNet.Types;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class PropertyArray : PropertyBase<IArkArray>
    {
        private static ILog _logger = LogManager.GetLogger(typeof(PropertyArray));

        public ArkName ArrayType { get; set; }

        public PropertyArray(string name, string typeName, IArkArray value, ArkName arrayType) : base(name, typeName, 0, value)
        {
            ArrayType = arrayType;
        }

        public PropertyArray(string name, string typeName, int index, IArkArray value, ArkName arrayType) : base(name, typeName, index, value)
        {

            ArrayType = arrayType;

        }

        public PropertyArray(ArkArchive archive, PropertyArgs args, bool propertyIsExcluded = false) : base(archive, args, propertyIsExcluded)
        {
            if (propertyIsExcluded)
            {
                archive.SkipName();
                archive.Position += DataSize;
                return;
            }

            ArrayType = archive.GetName();

            var position = archive.Position;

            try
            {
                _value = ArkArrayRegistry.read(archive, ArrayType, DataSize);

                if (_value == null)
                {
                    throw new UnreadablePropertyException();
                }
            }
            catch (UnreadablePropertyException)
            {
                archive.Position += DataSize;
                _logger.Error($"Unreadable ArrayProperty with name {Name}, skipping.");
                throw new UnreadablePropertyException();
            }
        }

        //public override Type ValueClass => typeof(IArkArray);

        public override IArkArray Value
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
        //@SuppressWarnings("unchecked") public <T> qowyn.ark.arrays.ArkArray<T> getTypedValue()
        //public ArkArray<T> getTypedValue<T>()
        //{
        //    get

        //      {
        //        return (ArkArray<T>)value;
        //    }
        //}

        //@SuppressWarnings("unchecked") public <T> qowyn.ark.arrays.ArkArray<T> getTypedValue(Class<T> clazz)
        //public ArkArray<T> getTypedValue<T>(Type<T> clazz)
        //{
        //    return value != null && value.ValueClass.IsAssignableFrom(clazz) ? (ArkArray<T>)value : null;
        //}

        //protected internal override int calculateAdditionalSize(bool nameTable)
        //{
        //    return ArkArchive.getNameLength(arrayType, nameTable);
        //}

        //public override int calculateDataSize(bool nameTable)
        //{
        //    return value.calculateSize(nameTable);
        //}

        //public override void CollectNames(ISet<string> nameTable)
        //{
        //    base.CollectNames(nameTable);
        //    nameTable.Add(ArrayType.Name);
        //    _value.CollectNames(nameTable);
        //}
    }
}
