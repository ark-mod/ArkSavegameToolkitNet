using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Arrays
{
    public class ArkArrayInteger : List<int?>, IArkArray<int?>
    {
        public ArkArrayInteger()
        {
        }

        public ArkArrayInteger(ArkArchive archive, int dataSize)
        {
            int size = archive.GetInt();
            Capacity = size;

            for (int n = 0; n < size; n++)
            {
                Add(archive.GetInt());
            }
        }

        public Type ValueClass => typeof(int?);

        //public int calculateSize(bool nameTable)
        //{
        //    return Integer.BYTES + this.Count * Integer.BYTES;
        //}

        public void CollectNames(ISet<string> nameTable)
        {
        }
    }
}
