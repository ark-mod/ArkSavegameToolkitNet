using ArkSavegameToolkitNet.DataTypes.Properties;
using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class ObjectReferenceArrayReader
    {
        public static ObjectReferenceArrayProperty Get(ArchiveReader ar)
        {
            var result = new ObjectReferenceArrayProperty { values = new List<ObjectReferenceProperty>() };

            ar.GetInt(out var size, "size");

            for (int n = 0; n < size; n++)
            {
                result.values.Add(ObjectReferencePropertyReader.Get(ar, 8));
            }

            return result;
        }
    }
}
