using ArkSavegameToolkitNet.DataTypes.Properties;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class Int8PropertyReader
    {
        public static Int8Property Get(ArchiveReader ar, bool propertyIsExcluded = false)
        {
            if (propertyIsExcluded)
            {
                ar.Advance(1);
                return null;
            }

            var result = new Int8Property();

            ar.GetByte(out result.value, "value");

            return result;
        }
    }
}
