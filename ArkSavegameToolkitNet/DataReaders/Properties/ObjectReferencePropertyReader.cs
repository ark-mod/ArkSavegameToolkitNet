using ArkSavegameToolkitNet.DataTypes.Properties;
using System;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class ObjectReferencePropertyReader
    {
        private const int TypeId = 0;
        private const int TypePath = 1;

        public static ObjectReferenceProperty Get(ArchiveReader ar, int dataSize, bool propertyIsExcluded = false)
        {
            if (propertyIsExcluded)
            {
                ar.Advance(dataSize);
                return null;
            }

            ObjectReferenceProperty result = null;

            if (dataSize >= 8)
            {
                ar.GetInt(out var type, "type");
                if (type == TypeId)
                {
                    ar.GetInt(out var id, "id");
                    result = new ObjectReferenceIdProperty { type = type, id = id };
                }
                else if (type == TypePath)
                {
                    result = new ObjectReferencePathProperty { type = type, path = ar.GetName("path") };
                }
                else
                {
                    throw new NotSupportedException("Object property wit unsupported type.");
                }
            }
            else if (dataSize == 4)
            {
                // Only seems to happen in Version 5
                ar.GetInt(out var id, "id");
                result = new ObjectReferenceIdProperty { type = TypeId, id = id };
            }
            else
            {
                ar.Advance(dataSize);
            }

            return result;
        }
    }
}
