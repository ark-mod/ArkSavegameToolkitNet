using ArkSavegameToolkitNet.DataTypes.Properties;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class StringArrayReader
    {
        public static ArrayProperty<string> Get(ArchiveReader ar)
        {
            var result = new ArrayProperty<string>();

            ar.GetInt(out var size, "size");

            for (int n = 0; n < size; n++)
            {
                result.values.Add(ar.GetString("values"));
            }

            return result;
        }
    }

    static class IntArrayReader
    {
        public static ArrayProperty<int> Get(ArchiveReader ar)
        {
            var result = new ArrayProperty<int>();

            ar.GetInt(out var size, "size");

            for (int n = 0; n < size; n++)
            {
                ar.GetInt(out var value, "value");
                result.values.Add(value);
            }

            return result;
        }
    }

    static class FloatArrayReader
    {
        public static ArrayProperty<float> Get(ArchiveReader ar)
        {
            var result = new ArrayProperty<float>();

            ar.GetInt(out var size, "size");

            for (int n = 0; n < size; n++)
            {
                ar.GetFloat(out var value, "value");
                result.values.Add(value);
            }

            return result;
        }
    }

    static class DoubleArrayReader
    {
        public static ArrayProperty<double> Get(ArchiveReader ar)
        {
            var result = new ArrayProperty<double>();

            ar.GetInt(out var size, "size");

            for (int n = 0; n < size; n++)
            {
                ar.GetDouble(out var value, "value");
                result.values.Add(value);
            }

            return result;
        }
    }

    static class ShortArrayReader
    {
        public static ArrayProperty<short> Get(ArchiveReader ar)
        {
            var result = new ArrayProperty<short>();

            ar.GetInt(out var size, "size");

            for (int n = 0; n < size; n++)
            {
                ar.GetShort(out var value, "value");
                result.values.Add(value);
            }

            return result;
        }
    }

    static class ULongArrayReader
    {
        public static ArrayProperty<ulong> Get(ArchiveReader ar)
        {
            var result = new ArrayProperty<ulong>();

            ar.GetInt(out var size, "size");

            for (int n = 0; n < size; n++)
            {
                ar.GetULong(out var value, "value");
                result.values.Add(value);
            }

            return result;
        }
    }

    static class BoolArrayReader
    {
        public static ArrayProperty<bool> Get(ArchiveReader ar)
        {
            var result = new ArrayProperty<bool>();

            ar.GetInt(out var size, "size");

            for (int n = 0; n < size; n++)
            {
                ar.GetByte(out var value, "value");
                result.values.Add(value != 0);
            }

            return result;
        }
    }
}
