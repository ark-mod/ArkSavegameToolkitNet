using ArkSavegameToolkitNet.DataTypes.Extras;

namespace ArkSavegameToolkitNet.DataReaders.Extras
{
    static class EmbeddedDataReader
    {
        public static EmbeddedData Get(ArchiveReader ar)
        {
            var result = new EmbeddedData();
            result.path = ar.GetString("path");

            ar.GetInt(out var partCount, "partCount");

            result.data = new byte[partCount][][];
            for (var part = 0; part < partCount; part++)
            {
                ar.GetInt(out var blobCount, "blobCount");
                var partData = new byte[blobCount][];

                for (var blob = 0; blob < blobCount; blob++)
                {
                    ar.GetInt(out var blobSize, "blobSize");
                    blobSize *= 4; // Array of 32 bit values
                    partData[blob] = new byte[blobSize];
                    ar.GetArray(partData[blob], 0, blobSize, "blob");
                }

                result.data[part] = partData;
            }

            return result;
        }

        public static void Skip(ArchiveReader ar)
        {
            ar.SkipString();

            ar.GetInt(out var partCount, "partCount");
            for (var part = 0; part < partCount; part++)
            {
                ar.GetInt(out var blobCount, "blobCount");
                for (var blob = 0; blob < blobCount; blob++)
                {
                    ar.GetInt(out var blobSize, "blobSize");
                    blobSize *= 4; // Array of 32 bit values
                    ar.Advance(blobSize);
                }
            }
        }

    }
}