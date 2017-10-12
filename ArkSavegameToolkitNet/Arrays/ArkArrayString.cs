using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Arrays
{
    public class ArkArrayString : List<string>, IArkArray<string>
    {
        public ArkArrayString()
        {
        }

        public ArkArrayString(ArkArchive archive, int dataSize)
        {
            var size = archive.GetInt();
            Capacity = size;

            for (int n = 0; n < size; n++)
            {
                Add(archive.GetString());
            }
        }
        public Type ValueClass => typeof(string);

        public void CollectNames(ISet<string> nameTable)
        {
        }

        //public int calculateSize(bool nameTable)
        //{
        //    int size = Integer.BYTES;

        //    size += stream().mapToInt(ArkArchive.getStringLength).sum();

        //    return size;
        //}
    }
}
