using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;
using System.Diagnostics;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class ArrayPropertyReader
    {
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

        public static ArrayProperty Get(ArchiveReader ar, int dataSize, bool propertyIsExcluded = false, ArkNameTree exclusivePropertyNameTreeChildNode = null)
        {
            if (propertyIsExcluded)
            {
                ar.SkipName();
                ar.Advance(dataSize);
                return null;
            }

            ArrayProperty result = null;

            try
            {
                ar._structureLog?.PushStack("ArrayProperty");

                var arrayType = ar.GetName("arrayType");

                var end = ar._position + dataSize;

                if (arrayType.Equals(_object)) result = ObjectReferenceArrayReader.Get(ar);
                else if (arrayType.Equals(_struct)) result = StructArrayReader.Get(ar, dataSize, exclusivePropertyNameTreeChildNode);
                else if (arrayType.Equals(_uint32) || arrayType.Equals(_int)) result = IntArrayReader.Get(ar);
                else if (arrayType.Equals(_uint16) || arrayType.Equals(_int16)) result = ShortArrayReader.Get(ar);
                else if (arrayType.Equals(_byte)) result = ByteArrayReader.Get(ar, dataSize);
                else if (arrayType.Equals(_int8)) result = Int8ArrayReader.Get(ar);
                else if (arrayType.Equals(_str)) result = StringArrayReader.Get(ar);
                else if (arrayType.Equals(_uint64)) result = ULongArrayReader.Get(ar);
                else if (arrayType.Equals(_bool)) result = BoolArrayReader.Get(ar);
                else if (arrayType.Equals(_float)) result = FloatArrayReader.Get(ar);
                else if (arrayType.Equals(_double)) result = DoubleArrayReader.Get(ar);
                else if (arrayType.Equals(_name)) result = NameArrayReader.Get(ar);
                else
                {
                    Debug.WriteLine($"Unknown Array Type {arrayType} at {ar._position:X}");
                    return null;
                }

                if (result == null)
                {
                    // skip array
                    //todo: logging
                }
                if (ar._position != end)
                {
                    ar.AdvanceTo(end);
                }

                return result;
            }
            finally
            {
                ar._structureLog?.PopStack();
            }
        }
    }
}
