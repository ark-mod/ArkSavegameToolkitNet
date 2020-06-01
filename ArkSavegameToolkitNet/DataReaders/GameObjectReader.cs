using ArkSavegameToolkitNet.DataReaders.Extras;
using ArkSavegameToolkitNet.DataTypes;
using System;
using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataReaders
{
    static class GameObjectReader
    {
        public static GameObject Get(ArchiveReader ar)
        {
            var result = new GameObject();

            try
            {
                ar._structureLog?.PushStack("GameObject");

                ar.GetLong(out var uuidMostSig, "uuidMostSig");
                ar.GetLong(out var uuidLeastSig, "uuidLeastSig");

                result.uuid = ar._uuidCache.Create((uuidMostSig, uuidLeastSig), () =>
                {
                    var bytes = new byte[16];
                    Array.Copy(BitConverter.GetBytes(uuidMostSig), bytes, 8);
                    Array.Copy(BitConverter.GetBytes(uuidLeastSig), 0, bytes, 8, 8);
                    return new Guid(bytes);
                });

                result.className = ar.GetName("className");
                ar._structureLog?.UpdateStack(result.className.Token);

                ar.GetBool(out result.isItem, "isItem");
                ar.GetInt(out var countNames, "countNames");
                result.names = new List<ArkName>();
                for (int nameIndex = 0; nameIndex < countNames; nameIndex++)
                {
                    result.names.Add(ar.GetName("names"));
                }

                ar.GetBool(out result.fromDataFile, "fromDataFile");
                ar.GetInt(out result.dataFileIndex, "dataFileIndex");

                ar.GetBool(out var hasLocationData, "hasLocationData");
                if (hasLocationData) result.location = LocationDataReader.Get(ar);

                ar.GetInt(out result.propertiesOffset, "propertiesOffset");

                ar.GetInt(out var shouldBeZero, "shouldBeZero");
                if (shouldBeZero != 0)
                {
                    //todo: not implemented
                }

                return result;
            }
            finally
            {
                ar._structureLog?.PopStack();
            }
        }
    }
}
