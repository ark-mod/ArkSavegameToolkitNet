using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Arrays
{
    public class ArkArrayBool : List<bool?>, IArkArray<bool?>
    {
        public ArkArrayBool()
        {
        }

        public ArkArrayBool(ArkArchive archive, int dataSize)
        {
            var size = archive.GetInt();
            Capacity = size;

            for (int n = 0; n < size; n++)
            {
                Add(archive.GetByte() != 0);
            }
        }

        public Type ValueClass => typeof(bool?);

        //public int calculateSize(bool nameTable)
        //{
        //    return Integer.BYTES + this.Count;
        //}

        public void CollectNames(ISet<string> nameTable)
        {
        }
    }
}
