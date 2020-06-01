using ArkSavegameToolkitNet.DataTypes.Properties;
using ArkSavegameToolkitNet.Exceptions;
using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class ByteArrayReader
    {
        public static ByteArrayProperty Get(ArchiveReader ar, int dataSize)
        {
            var result = new ByteArrayProperty { values = new List<byte>() };

            ar.GetInt(out var size, "size");

            if (size + 4 != dataSize)
            {
                throw new UnreadablePropertyException();
            }

            for (int n = 0; n < size; n++)
            {
                ar.GetByte(out var value, "value");
                result.values.Add(value);
            }

            return result;
        }
    }
}
