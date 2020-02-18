using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkSavegameToolkitNet.Arrays
{
    public class ArkArrayInt8 : List<sbyte?>, IArkArray<sbyte?>
    {
        public ArkArrayInt8()
        {
        }

        public ArkArrayInt8(ArkArchive archive, int dataSize)
        {
            var size = archive.GetInt();
            Capacity = size;

            AddRange(archive.GetSBytes(size).Cast<sbyte?>().ToArray());
        }

        public Type ValueClass => typeof(sbyte?);

        //public int calculateSize(bool nameTable)
        //{
        //    return Integer.BYTES + this.Count * Byte.BYTES;
        //}

        public void CollectNames(ISet<string> nameTable)
        {
        }
    }
}
