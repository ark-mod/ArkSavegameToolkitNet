using ArkSavegameToolkitNet.Exceptions;
using ArkSavegameToolkitNet.Property;
using ArkSavegameToolkitNet.Types;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Property
{
    public class PropertyRegistry
    {
        private static ILog _logger = LogManager.GetLogger(typeof(PropertyRegistry));

        private static readonly ArkName _int = ArkName.Create("IntProperty");
        private static readonly ArkName _uint32 = ArkName.Create("UInt32Property");
        private static readonly ArkName _int8 = ArkName.Create("Int8Property");
        private static readonly ArkName _int16 = ArkName.Create("Int16Property");
        private static readonly ArkName _uint16 = ArkName.Create("UInt16Property");
        private static readonly ArkName _uint64 = ArkName.Create("UInt64Property");
        private static readonly ArkName _bool = ArkName.Create("BoolProperty");
        private static readonly ArkName _byte = ArkName.Create("ByteProperty");
        private static readonly ArkName _float = ArkName.Create("FloatProperty");
        private static readonly ArkName _double = ArkName.Create("DoubleProperty");
        private static readonly ArkName _name = ArkName.Create("NameProperty");
        private static readonly ArkName _object = ArkName.Create("ObjectProperty");
        private static readonly ArkName _str = ArkName.Create("StrProperty");
        private static readonly ArkName _struct = ArkName.Create("StructProperty");
        private static readonly ArkName _array = ArkName.Create("ArrayProperty");
        private static readonly ArkName _text = ArkName.Create("TextProperty");

        public static IProperty readProperty(ArkArchive archive, ArkNameTree exclusivePropertyNameTree = null)
        {
            var name = archive.GetName();
            if (name == null || name.Equals(ArkName.EMPTY_NAME))
            {
                _logger.Error($"Property name is {(name == null ? "null" : "empty")}. Ignoring remaining properties.");
                throw new UnreadablePropertyException();
            }

            if (name.Equals(ArkName.NONE_NAME))
            {
                return null;
            }

            var propertyIsExcluded = exclusivePropertyNameTree?.ContainsKey(name) == false;

            var type = archive.GetName();
            var args = new PropertyArgs(name, type);

            IProperty result = null;
            if (type.Equals(_int) || type.Equals(_uint32)) result = new PropertyInt32(archive, args, propertyIsExcluded);
            else if (type.Equals(_bool)) result = new PropertyBool(archive, args, propertyIsExcluded);
            else if (type.Equals(_byte)) result = new PropertyByte(archive, args, propertyIsExcluded);
            else if (type.Equals(_float)) result = new PropertyFloat(archive, args, propertyIsExcluded);
            else if (type.Equals(_double)) result = new PropertyDouble(archive, args, propertyIsExcluded);
            else if (type.Equals(_int8)) result = new PropertyInt8(archive, args, propertyIsExcluded);
            else if (type.Equals(_int16) || type.Equals(_uint16)) result = new PropertyInt16(archive, args, propertyIsExcluded);
            else if (type.Equals(_uint64)) result = new PropertyInt64(archive, args, propertyIsExcluded);
            else if (type.Equals(_name)) result = new PropertyName(archive, args, propertyIsExcluded);
            else if (type.Equals(_object)) result = new PropertyObject(archive, args, propertyIsExcluded);
            else if (type.Equals(_str)) result = new PropertyStr(archive, args, propertyIsExcluded);
            else if (type.Equals(_struct)) result = new PropertyStruct(archive, args, propertyIsExcluded, exclusivePropertyNameTree == null || propertyIsExcluded ? null : exclusivePropertyNameTree[name]);
            else if (type.Equals(_array)) result = new PropertyArray(archive, args, propertyIsExcluded);
            else if (type.Equals(_text)) result = new PropertyText(archive, args, propertyIsExcluded);
            else
            {
                _logger.Error($"Unknown property type {type} near {archive.Position:X}. Ignoring remaining properties.");
                throw new UnreadablePropertyException();
            }

            if (propertyIsExcluded) return ExcludedProperty.Instance;

            return result;
        }
    }
}
