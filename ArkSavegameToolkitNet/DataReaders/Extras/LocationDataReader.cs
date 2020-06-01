using ArkSavegameToolkitNet.DataTypes.Extras;

namespace ArkSavegameToolkitNet.DataReaders.Extras
{
    static class LocationDataReader
    {
        public static LocationData Get(ArchiveReader ar)
        {
            var result = new LocationData();

            ar.GetFloat(out result.x, "x");
            ar.GetFloat(out result.y, "y");
            ar.GetFloat(out result.z, "z");
            ar.GetFloat(out result.pitch, "pitch");
            ar.GetFloat(out result.yaw, "yaw");
            ar.GetFloat(out result.roll, "roll");

            return result;
        }

        public static void Skip(ArchiveReader ar)
        {
            ar.Advance(/* float size */ 4 * 6);
        }
    }
}
