using ArkSavegameToolkitNet.DataTypes.Properties;
using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class Int8ArrayReader
    {
        public static Int8ArrayProperty Get(ArchiveReader ar)
        {
            var result = new Int8ArrayProperty { values = new List<byte>() };

            ar.GetInt(out var size, "size");

            for (int n = 0; n < size; n++)
            {
                ar.GetByte(out var value, "value");
                result.values.Add(value);
            }

            return result;
        }
    }
}
