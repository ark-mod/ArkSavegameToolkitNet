using ArkSavegameToolkitNet.DataTypes.Properties;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class ColorStructReader
    {
        public static ColorStructProperty Get(ArchiveReader ar)
        {
            var result = new ColorStructProperty();

            ar.GetSByte(out result.b, "b");
            ar.GetSByte(out result.g, "g");
            ar.GetSByte(out result.r, "r");
            ar.GetSByte(out result.a, "a");

            return result;
        }
    }
}
