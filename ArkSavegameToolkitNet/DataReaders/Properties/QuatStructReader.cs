using ArkSavegameToolkitNet.DataTypes.Properties;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class QuatStructReader
    {
        public static QuatStructProperty Get(ArchiveReader ar)
        {
            var result = new QuatStructProperty();

            ar.GetFloat(out result.x, "x");
            ar.GetFloat(out result.y, "y");
            ar.GetFloat(out result.z, "z");
            ar.GetFloat(out result.w, "w");

            return result;
        }
    }
}
