using System.Collections.Generic;

namespace ArkSavegameToolkitNet.DataTypes.Extras
{
    public struct HibernationEntry : IDataEntry
    {
        public float x;
        public float y;
        public float z;
        public ArkName[] zoneVolumes;
        public List<GameObject> objects;
        public int classIndex;
        public string[] nameTable;
    }
}
