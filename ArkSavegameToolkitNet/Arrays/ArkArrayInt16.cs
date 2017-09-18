using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Arrays
{
    public class ArkArrayInt16 : List<short?>, IArkArray<short?>
    {
        public ArkArrayInt16()
        {
        }

        public ArkArrayInt16(ArkArchive archive, int dataSize)
        {
            var size = archive.GetInt();

            for (int n = 0; n < size; n++)
            {
                Add(archive.GetShort());
            }
        }

        public Type ValueClass => typeof(short?);

        //public int calculateSize(bool nameTable)
        //{
        //    return Integer.BYTES + this.Count * Short.BYTES;
        //}

        public void CollectNames(ISet<string> nameTable)
        {
        }
    }
}
