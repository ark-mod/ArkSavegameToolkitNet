using ArkSavegameToolkitNet.DataTypes.Properties;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class VectorStructReader
    {
        public static VectorStructProperty Get(ArchiveReader ar)
        {
            var result = new VectorStructProperty();

            ar.GetFloat(out result.x, "x");
            ar.GetFloat(out result.y, "y");
            ar.GetFloat(out result.z, "z");

            return result;
        }
    }
}
