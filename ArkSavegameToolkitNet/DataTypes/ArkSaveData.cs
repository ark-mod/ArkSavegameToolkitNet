using System;

namespace ArkSavegameToolkitNet.DataTypes
{
    public class ArkSaveData : IDataEntry
    {
        public string fileName;
        public long size;
        public DateTime savedAt;
        public short saveVersion;
        public float gameTime;
        public int saveCount;
        public string mapName;
    }
}
