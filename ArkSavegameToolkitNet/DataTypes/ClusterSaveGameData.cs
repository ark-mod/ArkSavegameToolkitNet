using System;

namespace ArkSavegameToolkitNet.DataTypes
{
    public class ClusterSaveGameData : IDataEntry
    {
        public string fileName;
        public string steamId;
        public long size;
        public DateTime savedAt;
        public int saveVersion;
        public GameObject[] objects;
    }
}
