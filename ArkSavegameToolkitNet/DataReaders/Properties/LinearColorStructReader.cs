using ArkSavegameToolkitNet.DataTypes.Properties;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class LinearColorStructReader
    {
        public static LinearColorStructProperty Get(ArchiveReader ar)
        {
            var result = new LinearColorStructProperty();

            ar.GetFloat(out result.r, "r");
            ar.GetFloat(out result.g, "g");
            ar.GetFloat(out result.b, "b");
            ar.GetFloat(out result.a, "a");

            return result;
        }
    }
}
