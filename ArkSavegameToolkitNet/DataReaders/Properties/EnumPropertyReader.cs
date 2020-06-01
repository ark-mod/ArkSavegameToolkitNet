using ArkSavegameToolkitNet.DataTypes;
using ArkSavegameToolkitNet.DataTypes.Properties;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class EnumPropertyReader
    {
        public static EnumProperty Get(ArchiveReader ar, bool propertyIsExcluded = false)
        {
            EnumProperty result = null;

            var enumName = ar.GetName("enumName");
            var fromEnum = !enumName.Equals(ArkName.None);
            if (fromEnum)
            {
                if (propertyIsExcluded)
                {
                    ar.SkipName();
                    return null;
                }

                result = new EnumNameProperty { @enum = enumName, value = ar.GetName("value") };
            }
            else
            {
                if (propertyIsExcluded)
                {
                    ar.Advance(1);
                    return null;
                }

                ar.GetByte(out var value, "value");
                result = new EnumByteProperty { @enum = enumName, value = value };
            }

            return result;
        }
    }
}
