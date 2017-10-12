using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Arrays
{
    public class ArkArrayLong : List<long?>, IArkArray<long?>
    {
        public ArkArrayLong()
        {
        }

        public ArkArrayLong(ArkArchive archive, int dataSize)
        {
            var size = archive.GetInt();
            Capacity = size;

            for (int n = 0; n < size; n++)
            {
                this.Add(archive.GetLong());
            }
        }

        public Type ValueClass => typeof(long?);

        //public int calculateSize(bool nameTable)
        //{
        //    return Integer.BYTES + this.Count * Long.BYTES;
        //}

        public void CollectNames(ISet<string> nameTable)
        {
        }
    }
}
