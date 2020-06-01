using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;
using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class StructArrayReader
    {
        public static StructArrayProperty Get(ArchiveReader ar, int dataSize, ArkNameTree exclusivePropertyNameTreeChildNode = null)
        {
            var result = new StructArrayProperty { values = new List<StructProperty>() };

            ar.GetInt(out var size, "size");

            ArkName structType;
            if (size * 4 + 4 == dataSize)
            {
                structType = ArkName.Create("Color");
            }
            else if (size * 12 + 4 == dataSize)
            {
                structType = ArkName.Create("Vector");
            }
            else if (size * 16 + 4 == dataSize)
            {
                structType = ArkName.Create("LinearColor");
            }
            else
            {
                structType = null;
            }

            if (structType != null)
            {
                for (int n = 0; n < size; n++)
                {
                    result.values.Add(StructPropertyReader.Get(ar, structType, exclusivePropertyNameTreeChildNode));
                }
            }
            else
            {
                for (int n = 0; n < size; n++)
                {
                    result.values.Add(PropertyListStructReader.Get(ar, exclusivePropertyNameTreeChildNode));
                }
            }

            return result;
        }
    }
}
