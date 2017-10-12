using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Arrays
{
    public class ArkArrayFloat : List<float?>, IArkArray<float?>
    {
        public ArkArrayFloat()
        {
        }

        public ArkArrayFloat(ArkArchive archive, int dataSize)
        {
            var size = archive.GetInt();
            Capacity = size;

            for (int n = 0; n < size; n++)
            {
                Add(archive.GetFloat());
            }
        }

        public Type ValueClass => typeof(float?);

        //public int calculateSize(bool nameTable)
        //{
        //    return Integer.BYTES + this.Count * Float.BYTES;
        //}

        public void CollectNames(ISet<string> nameTable)
        {
        }
    }
}
