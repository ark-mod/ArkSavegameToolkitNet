using ArkSavegameToolkitNet.DataTypes.Properties;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class Vector2dStructReader
    {
        public static Vector2dStructProperty Get(ArchiveReader ar)
        {
            var result = new Vector2dStructProperty();

            ar.GetFloat(out result.x, "x");
            ar.GetFloat(out result.y, "y");

            return result;
        }
    }
}
