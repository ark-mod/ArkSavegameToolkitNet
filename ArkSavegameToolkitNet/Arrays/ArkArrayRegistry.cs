using ArkSavegameToolkitNet.Types;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Arrays
{
    public sealed class ArkArrayRegistry
    {
        private static ILog _logger = LogManager.GetLogger(typeof(ArkArrayRegistry));

        private static readonly ArkName _object = ArkName.Create("ObjectProperty");
        private static readonly ArkName _struct = ArkName.Create("StructProperty");
        private static readonly ArkName _uint32 = ArkName.Create("UInt32Property");
        private static readonly ArkName _int = ArkName.Create("IntProperty");
        private static readonly ArkName _uint16 = ArkName.Create("UInt16Property");
        private static readonly ArkName _int16 = ArkName.Create("Int16Property");
        private static readonly ArkName _byte = ArkName.Create("ByteProperty");
        private static readonly ArkName _int8 = ArkName.Create("Int8Property");
        private static readonly ArkName _str = ArkName.Create("StrProperty");
        private static readonly ArkName _uint64 = ArkName.Create("UInt64Property");
        private static readonly ArkName _bool = ArkName.Create("BoolProperty");
        private static readonly ArkName _float = ArkName.Create("FloatProperty");
        private static readonly ArkName _double = ArkName.Create("DoubleProperty");
        private static readonly ArkName _name = ArkName.Create("NameProperty");

        public static IArkArray read(ArkArchive archive, ArkName arrayType, int size)
        {
            if (arrayType.Equals(_object)) return new ArkArrayObjectReference(archive, size);
            else if (arrayType.Equals(_struct)) return new ArkArrayStruct(archive, size);
            else if (arrayType.Equals(_uint32) || arrayType.Equals(_int)) return new ArkArrayInteger(archive, size);
            else if (arrayType.Equals(_uint16) || arrayType.Equals(_int16)) return new ArkArrayInt16(archive, size);
            else if (arrayType.Equals(_byte)) return new ArkArrayByte(archive, size);
            else if (arrayType.Equals(_int8)) return new ArkArrayInt8(archive, size);
            else if (arrayType.Equals(_str)) return new ArkArrayString(archive, size);
            else if (arrayType.Equals(_uint64)) return new ArkArrayLong(archive, size);
            else if (arrayType.Equals(_bool)) return new ArkArrayBool(archive, size);
            else if (arrayType.Equals(_float)) return new ArkArrayFloat(archive, size);
            else if (arrayType.Equals(_double)) return new ArkArrayDouble(archive, size);
            else if (arrayType.Equals(_name)) return new ArkArrayName(archive, size);
            else
            {
                _logger.Warn($"Unknown Array Type {arrayType} at {archive.Position:X}");
                return null;
            }
        }
    }
}
