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

        public static IProperty readProperty(ArkArchive archive)
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

            var type = archive.GetName();
            var args = new PropertyArgs(name, type);

            if (type.Equals(_int) || type.Equals(_uint32)) return new PropertyInt32(archive, args);
            else if (type.Equals(_bool)) return new PropertyBool(archive, args);
            else if (type.Equals(_byte)) return new PropertyByte(archive, args);
            else if (type.Equals(_float)) return new PropertyFloat(archive, args);
            else if (type.Equals(_double)) return new PropertyDouble(archive, args);
            else if (type.Equals(_int8)) return new PropertyInt8(archive, args);
            else if (type.Equals(_int16) || type.Equals(_uint16)) return new PropertyInt16(archive, args);
            else if (type.Equals(_uint64)) return new PropertyInt64(archive, args);
            else if (type.Equals(_name)) return new PropertyName(archive, args);
            else if (type.Equals(_object)) return new PropertyObject(archive, args);
            else if (type.Equals(_str)) return new PropertyStr(archive, args);
            else if (type.Equals(_struct)) return new PropertyStruct(archive, args);
            else if (type.Equals(_array)) return new PropertyArray(archive, args);
            else if (type.Equals(_text)) return new PropertyText(archive, args);
            else
            {
                _logger.Error($"Unknown property type {type} near {archive.Position:X}. Ignoring remaining properties.");
                throw new UnreadablePropertyException();
            }
        }
    }
}
