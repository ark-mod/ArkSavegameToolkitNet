using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class NamePropertyReader
    {
        public static ValueProperty<ArkName> Get(ArchiveReader ar, bool propertyIsExcluded = false)
        {
            if (propertyIsExcluded)
            {
                ar.SkipName();
                return null;
            }

            var result = new ValueProperty<ArkName>();

            result.value = ar.GetName("value");

            return result;
        }
    }

    static class StringPropertyReader
    {
        public static ValueProperty<string> Get(ArchiveReader ar, bool propertyIsExcluded = false)
        {
            if (propertyIsExcluded)
            {
                ar.SkipString();
                return null;
            }

            var result = new ValueProperty<string>();

            result.value = ar.GetString("value");

            return result;
        }
    }

    static class IntPropertyReader
    {
        public static ValueProperty<int> Get(ArchiveReader ar, bool propertyIsExcluded = false)
        {
            if (propertyIsExcluded)
            {
                ar.Advance(4);
                return null;
            }

            var result = new ValueProperty<int>();

            ar.GetInt(out result.value, "value");

            return result;
        }
    }

    static class UIntPropertyReader
    {
        public static ValueProperty<uint> Get(ArchiveReader ar, bool propertyIsExcluded = false)
        {
            if (propertyIsExcluded)
            {
                ar.Advance(4);
                return null;
            }

            var result = new ValueProperty<uint>();

            ar.GetUInt(out result.value, "value");

            return result;
        }
    }

    static class FloatPropertyReader
    {
        public static ValueProperty<float> Get(ArchiveReader ar, bool propertyIsExcluded = false)
        {
            if (propertyIsExcluded)
            {
                ar.Advance(4);
                return null;
            }

            var result = new ValueProperty<float>();

            ar.GetFloat(out result.value, "value");

            return result;
        }
    }

    static class DoublePropertyReader
    {
        public static ValueProperty<double> Get(ArchiveReader ar, bool propertyIsExcluded = false)
        {
            if (propertyIsExcluded)
            {
                ar.Advance(8);
                return null;
            }

            var result = new ValueProperty<double>();

            ar.GetDouble(out result.value, "value");

            return result;
        }
    }

    static class ShortPropertyReader
    {
        public static ValueProperty<short> Get(ArchiveReader ar, bool propertyIsExcluded = false)
        {
            if (propertyIsExcluded)
            {
                ar.Advance(2);
                return null;
            }

            var result = new ValueProperty<short>();

            ar.GetShort(out result.value, "value");

            return result;
        }
    }

    static class UShortPropertyReader
    {
        public static ValueProperty<ushort> Get(ArchiveReader ar, bool propertyIsExcluded = false)
        {
            if (propertyIsExcluded)
            {
                ar.Advance(2);
                return null;
            }

            var result = new ValueProperty<ushort>();

            ar.GetUShort(out result.value, "value");

            return result;
        }
    }

    static class ULongPropertyReader
    {
        public static ValueProperty<ulong> Get(ArchiveReader ar, bool propertyIsExcluded = false)
        {
            if (propertyIsExcluded)
            {
                ar.Advance(8);
                return null;
            }

            var result = new ValueProperty<ulong>();

            ar.GetULong(out result.value, "value");

            return result;
        }
    }

    static class BoolPropertyReader
    {
        public static ValueProperty<bool> Get(ArchiveReader ar, bool propertyIsExcluded = false)
        {
            if (propertyIsExcluded)
            {
                ar.Advance(1);
                return null;
            }

            var result = new ValueProperty<bool>();

            ar.GetByte(out var tmp, "tmp");
            result.value = tmp != 0;

            return result;
        }
    }
}
