using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;
using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class NameArrayReader
    {
        public static NameArrayProperty Get(ArchiveReader ar)
        {
            var result = new NameArrayProperty { values = new List<ArkName>() };

            ar.GetInt(out var size, "size");

            for (int n = 0; n < size; n++)
            {
                result.values.Add(ar.GetName("values"));
            }

            return result;
        }
    }
}
