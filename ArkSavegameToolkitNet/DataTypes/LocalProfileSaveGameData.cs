using System;

namespace ArkSavegameToolkitNet.DataTypes
{
    public class LocalProfileSaveGameData : IDataEntry
    {
        public string fileName;
        public long size;
        public DateTime savedAt;
        public int saveVersion;
        public GameObject[] objects;
    }
}
