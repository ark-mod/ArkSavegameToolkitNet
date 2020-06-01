using ArkSavegameToolkitNet.DataReaders.Properties;
using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;
using ArkSavegameToolkitNet.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;

namespace ArkSavegameToolkitNet.DataReaders
{
    static class PropertyReader
    {
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

        public static IEnumerable<PropertyBase> GetMany(ArchiveReader ar, ArkNameTree exclusivePropertyNameTreeChildNode = null)
        {
            PropertyBase prop = null;
            while ((prop = readProperty(ar, exclusivePropertyNameTreeChildNode)) != null)
            {
                if (prop == ExcludedProperty.Instance) continue;

                yield return prop;
            }
        }

        private static PropertyBase readProperty(ArchiveReader ar, ArkNameTree exclusivePropertyNameTreeChildNode = null)
        {
            try
            {
                ar._structureLog?.PushStack("Property");

                var name = ar.GetName("name");
                var index = 0;

                if (name == null)
                {
                    throw new UnreadablePropertyException();
                }

                // we hit the last property and are finished reading this properties section
                if (name.Equals(ArkName.None)) return null;

                var epnt = (exclusivePropertyNameTreeChildNode ?? ar._exclusivePropertyNameTree);
                var propertyIsExcluded = epnt?.ContainsKey(name) == false;

                var type = ar.GetName("type");

                ar.GetInt(out var dataSize, "dataSize");

                ar.GetInt(out index, "index");

                if (type == null)
                {
                    ar._structureLog?.UpdateStack(name.Token);

                    Debug.WriteLine($"Property type was null for '{name}' near {ar._position:X}.");
                    return null;
                } else ar._structureLog?.UpdateStack($"{type} ({name})");

                PropertyBase result = null;
                if (type.Equals(_int)) result = IntPropertyReader.Get(ar, propertyIsExcluded);
                else if (type.Equals(_uint32)) result = UIntPropertyReader.Get(ar, propertyIsExcluded);
                else if (type.Equals(_bool)) result = BoolPropertyReader.Get(ar, propertyIsExcluded);
                else if (type.Equals(_byte)) result = EnumPropertyReader.Get(ar, propertyIsExcluded);
                else if (type.Equals(_float)) result = FloatPropertyReader.Get(ar, propertyIsExcluded);
                else if (type.Equals(_double)) result = DoublePropertyReader.Get(ar, propertyIsExcluded);
                else if (type.Equals(_int8)) result = Int8PropertyReader.Get(ar, propertyIsExcluded);
                else if (type.Equals(_int16)) result = ShortPropertyReader.Get(ar, propertyIsExcluded);
                else if (type.Equals(_uint16)) result = UShortPropertyReader.Get(ar, propertyIsExcluded);
                else if (type.Equals(_uint64)) result = ULongPropertyReader.Get(ar, propertyIsExcluded);
                else if (type.Equals(_name)) result = NamePropertyReader.Get(ar, propertyIsExcluded);
                else if (type.Equals(_object)) result = ObjectReferencePropertyReader.Get(ar, dataSize, propertyIsExcluded);
                else if (type.Equals(_str)) result = StringPropertyReader.Get(ar, propertyIsExcluded);
                else if (type.Equals(_struct)) result = StructPropertyReader.Get(ar, dataSize, propertyIsExcluded, epnt == null || propertyIsExcluded ? null : epnt[name]);
                else if (type.Equals(_array)) result = ArrayPropertyReader.Get(ar, dataSize, propertyIsExcluded, epnt == null || propertyIsExcluded ? null : epnt[name]);
                else if (type.Equals(_text)) result = TextPropertyReader.Get(ar, dataSize, propertyIsExcluded);
                else
                {
                    Debug.WriteLine($"Unknown property type {type} near {ar._position:X}.");
                }

                if (propertyIsExcluded) return ExcludedProperty.Instance;

                result.index = index;
                result.name = index == 0 ? name : ar._stringCache.AddName(name.Token, index);


                return result;
            }
            finally
            {
                ar._structureLog?.PopStack();
            }
        }
    }
}
