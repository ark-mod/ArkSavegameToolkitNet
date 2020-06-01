using ArkSavegameToolkitNet.DataTypes.Properties;

namespace ArkSavegameToolkitNet.DataReaders.Properties
{
    static class UniqueNetIdRepltructReader
    {
        public static UniqueNetIdReplStructProperty Get(ArchiveReader ar)
        {
            var result = new UniqueNetIdReplStructProperty();

            ar.GetInt(out result.unk, "unk");
            result.netId = ar.GetString("netId");

            return result;
        }
    }
}
