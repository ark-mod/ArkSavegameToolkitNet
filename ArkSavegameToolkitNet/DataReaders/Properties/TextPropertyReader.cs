using ArkSavegameToolkitNet.DataTypes.Properties;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class TextPropertyReader
    {
        public static ValueProperty<string> Get(ArchiveReader ar, int dataSize, bool propertyIsExcluded = false)
        {
            if (propertyIsExcluded)
            {
                ar.Advance(dataSize);
                return null;
            }

            var result = new ValueProperty<string>();

            var base64StringData = new byte[dataSize];
            ar.GetArray(base64StringData, 0, dataSize, "base64String");

            result.value = System.Convert.ToBase64String(base64StringData);

            return result;
        }
    }
}
